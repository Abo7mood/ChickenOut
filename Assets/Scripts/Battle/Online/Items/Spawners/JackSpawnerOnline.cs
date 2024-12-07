using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Items;

namespace ChickenOut.Battle.Online.Items
{
    public class JackSpawnerOnline : SpawnerOnline
    {
        #region Stuff
        public static JackSpawnerOnline instance;

        List<JackBoxPickupOnline> _jackBoxes = new List<JackBoxPickupOnline>();
        [SerializeField] JackBoxPickupOnline _jackBoxPrefab;
        [SerializeField] Transform _holder;

        [SerializeField] float _mapEdge = 17f, _mapHeghit = 9f;
        float _speed;
        int _maxBoxesAllowed;
        #endregion

        #region Starting
        protected override void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;

            base.Awake();
        }

        protected override void OnGamePrepared(GameMode mode)
        {
            JackBoxModeInfo info = mode.JackBoxInfo;

            _enableMyItems = info.enableJack;
            if (!_enableMyItems)
            {
                enabled = false;
                return;
            }

            OriginalChargeTime = info.jackChargeTime;
            _maxBoxesAllowed = info.maxJackBoxesAllowed;
            _speed = info.jackSpeed;

            _jackBoxes.Clear();
            for (int i = 0; i < _maxBoxesAllowed; i++)
            {
                JackBoxPickupOnline box = Instantiate(_jackBoxPrefab, Vector2.one * 100f, Quaternion.identity, _holder);

                box.Setup(_speed, i);
                _jackBoxes.Add(box);
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
            if (Cant || NotYet || _jackBoxes == null)
                return;

            for (int i = 0; i < _jackBoxes.Count; i++)
                if (_jackBoxes[i].gameObject.activeInHierarchy)
                {
                    if (Mathf.Abs(_jackBoxes[i].transform.position.x) > _mapEdge || 
                        Mathf.Abs(_jackBoxes[i].transform.position.y) > _mapHeghit)
                        Spawn(i);
                }
                else if (!_jackBoxes[i].gameObject.activeInHierarchy && IsReady)
                    Spawn(i);
        }

        void Spawn(int index)
        {
            _photonView.RPC(nameof(SpawnJackBoxRPC), RpcTarget.All, index, GetRandomPoint());
            ResetTimer();
        }
        [PunRPC] void SpawnJackBoxRPC(int index, Vector2 vector) => _jackBoxes[index].SpawnJack(vector, GetDirection(vector));

        public void CallInteractingRPC(int index) => _photonView.RPC(nameof(JackBoxInteractingRPC), RpcTarget.All, index);
        [PunRPC] void JackBoxInteractingRPC(int index) => _jackBoxes[index].CallCollect();
        #endregion

        #region Getting Points
        Vector3 GetDirection(Vector2 vector)
        {
            if (vector.x == _mapEdge)
                return Vector3.left;

            else if (vector.x == -_mapEdge)
                return Vector3.right;

            else if (vector.y == _mapHeghit)
                return Vector3.down;

            else if (vector.y == -_mapHeghit)
                return Vector3.up;

            else
                return Vector3.right;
        }
        Vector2 GetRandomPoint()
        {
            return Random.Range(1, 5) switch
            {
                1 => new Vector2(Random.Range(-_mapEdge, _mapEdge), _mapHeghit),
                2 => new Vector2(Random.Range(-_mapEdge, _mapEdge), -_mapHeghit),
                3 => new Vector2(_mapEdge, Random.Range(-_mapHeghit, _mapHeghit)),
                4 => new Vector2(-_mapEdge, Random.Range(-_mapHeghit, _mapHeghit)),
            };
        }
        #endregion
    }
}