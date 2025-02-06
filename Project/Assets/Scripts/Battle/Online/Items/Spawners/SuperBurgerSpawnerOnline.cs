using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Items;

namespace ChickenOut.Battle.Online.Items
{
    public class SuperBurgerSpawnerOnline : SpawnerOnline
    {
        #region Stuff
        public static SuperBurgerSpawnerOnline instance;

        [SerializeField] SuperBurgerPickupOnline _superBurgerPrefab;
        SuperBurgerPickupOnline _superBurger;
        [SerializeField] Transform _holder;
        MoveLine[] _moveLines;

        int _currentPoint;
        float _speed;
        bool _wasOn;

        Vector2 _target;
        #endregion

        #region Starting
        protected override void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;

            _moveLines = FindObjectsOfType<MoveLine>();

            base.Awake();
        }

        protected override void OnGamePrepared(GameMode mode)
        {
            SuperBurgerModeInfo info = mode.SuperBurgerInfo;

            _enableMyItems = info.enableBurger;
            if (!_enableMyItems)
            {
                enabled = false;
                return;
            }

            OriginalChargeTime = info.superBurgerChargeTime;
            _speed = info.superBurgerSpeed;

            _superBurger = Instantiate(_superBurgerPrefab, Vector2.one * 100f, Quaternion.identity, _holder);
            _currentPoint = 0;
            _target = _moveLines[_currentPoint].transform.position;
        }
        protected override void OnGameStarted()
        {
            if (Cant || !_enableMyItems)
                return;

            ResetTimer();
        }
        #endregion

        #region Moving
        void Update()
        {
            if (NotYet || _superBurger == null)
                return;

            if (_superBurger.gameObject.activeInHierarchy)
                Moving();

            else
            {
                if (Cant)
                    return;

                if (_wasOn)
                    Reseting();

                else if (IsReady)
                    Spawn();
            }
        }

        void Moving()
        {
            _wasOn = true;
            _superBurger.transform.position = Vector2.MoveTowards(_superBurger.transform.position, _target, _speed * Time.deltaTime);

            if (Vector2.Distance(_superBurger.transform.position, _target) <= .1f)
            {
                _currentPoint.Next(_moveLines.Length);
                _target = _moveLines[_currentPoint].transform.position;
            }
        }
        #endregion

        #region Reseting
        void Reseting()
        {
            _wasOn = false;

            if (Cant)
                return;

            ResetTimer();
            int random = Random.Range(0, _moveLines.Length);
            _photonView.RPC(nameof(TakeCurrentPointRPC), RpcTarget.All, random);
        }
        [PunRPC] void TakeCurrentPointRPC(int point) => _currentPoint = point;

        public void CallInteractingRPC() => _photonView.RPC(nameof(SuperBurgerInteractingRPC), RpcTarget.All);
        [PunRPC] void SuperBurgerInteractingRPC() => _superBurger.CallCollect();
        #endregion

        #region Spawning
        void Spawn()
        {
            Vector2 spawnPosition = _moveLines[_currentPoint].transform.position;
            _photonView.RPC(nameof(SpawnSuperBurgerRPC), RpcTarget.All, spawnPosition.ToFloats());
        }
        [PunRPC] void SpawnSuperBurgerRPC(float[] spawnPosition)
        {
            _superBurger.SpawnSuperBurger(spawnPosition.ToVector2());
            Cinemachineshake2.instance.ShakeBurgerRespawn();
        }
        #endregion
    }
}