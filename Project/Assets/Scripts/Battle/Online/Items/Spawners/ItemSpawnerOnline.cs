using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Items;

namespace ChickenOut.Battle.Online.Items
{
    public class ItemSpawnerOnline : SpawnerOnline
    {
        #region Stuff
        public static ItemSpawnerOnline instance;

        List<ItemPickupOnline> _itemPickups = new List<ItemPickupOnline>();
        [SerializeField] ItemPickupOnline _itemPickupPrefab;
        [SerializeField] Transform _holder;
        SpawnLine[] _spawnLines;
        Item[] _items;

        float _itemDisappearTime;
        int _nextItem;
        #endregion

        #region Starting
        protected override void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;

            base.Awake();

            _spawnLines = FindObjectsOfType<SpawnLine>();
        }
        protected override void OnGamePrepared(GameMode mode)
        {
            ItemsModeInfo info = mode.ItemsInfo;

            _enableMyItems = info.enableItems;
            if (!_enableMyItems)
            {
                enabled = false;
                return;
            }

            OriginalChargeTime = info.itemChargeTime;
            _itemDisappearTime = info.itemDisappearTime;
            _items = info.items;

            _itemPickups.Clear();
            for (int i = 0; i < _items.Length; i++)
            {
                ItemPickupOnline pickup = Instantiate(_itemPickupPrefab, Vector2.one * 100f, Quaternion.identity, _holder);

                pickup.Setup(i, _itemDisappearTime, _items[i]);
                _itemPickups.Add(pickup);
            }
        }
        protected override void OnGameStarted()
        {
            if (Cant || !_enableMyItems)
                return;

            ResetTimer();
        }
        #endregion

        #region Spawning
        void Update()
        {
            if (Cant || NotYet || _itemPickups == null)
                return;

            if (IsReady && _itemPickups.Any(i => !i.gameObject.activeInHierarchy))
                SpawnItem();
        }
        void SpawnItem()
        {
            int lineIndex = Random.Range(0, _spawnLines.Length);
            Vector2 spawnPosition = _spawnLines[lineIndex].GetRandomPoint();

            _photonView.RPC(nameof(SpawnItemRPC), RpcTarget.All, spawnPosition.ToFloats());
            ResetTimer();
        }
        [PunRPC] void SpawnItemRPC(float [] spawnPosition) => _itemPickups[_nextItem].Spawn(spawnPosition.ToVector2());

        public void CallInteractingRPC(int index) => _photonView.RPC(nameof(ItemInteractingRPC), RpcTarget.All, index);
        [PunRPC] void ItemInteractingRPC(int index) => _itemPickups[index].CallCollect();
        #endregion

        #region Reseting
        protected override void ResetTimer(float newChargeTime = 0)
        {
            base.ResetTimer(newChargeTime);

            int nm;
            do
            {
                nm = Random.Range(0, _itemPickups.Count);
            } while (_itemPickups[nm].gameObject.activeInHierarchy && _itemPickups.Any(i => !i.gameObject.activeInHierarchy));

            _photonView.RPC(nameof(TakeNextItemRPC), RpcTarget.All, nm);
        }
        [PunRPC] void TakeNextItemRPC(int nm) => _nextItem = nm;
        #endregion
    }
}