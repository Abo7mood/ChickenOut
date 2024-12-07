using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Combatings.Offence;
using ChickenOut.Battle.Online.Managers;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Chickens;
using ChickenOut.Battle.Online.Combatings.Defence;

namespace ChickenOut.Battle.Online.Chickens
{
    public class ChickenCombater : Battle.Chickens.ChickenCombater, IPunObservable
    {
        #region Stuff
        PhotonView _photonView;
        protected float aimAngle;
        #endregion

        #region Starting
        protected override void Awake()
        {
            _shield = GetComponentInChildren<Shield>();
            _photonView = GetComponent<PhotonView>();

            base.Awake();
        }
        protected override void Start()
        {
            GameManagerOnline _gameManager = GameManagerOnline.instance;
            _gameManager.onGamePrepared += OnGamePrepared;
            _gameManager.onGameEnded += OnGameEnded;

            _fightManager = FightManagerOnline.instance;

            onChangeCombatingCalled += CallUpdateCombatingRPC;
            onDoGunCooldownCalled += CallDoGunCooldown;
            onAttackingCalled += Attacking;

            if (_itemPicker.enabled)
            {
                _itemPicker.onFireRateBoostCollected += CallFireRateBoostRPC;
                _itemPicker.onSuperBurgerCollected += CallBurgerBulletRPC;
                _itemPicker.onPowerCollected += CallDoublePowerRPC;
            }

            base.Start();
        }

        void OnGamePrepared(GameMode mode)
        {
            CombaterModeInfo info = mode.CombaterInfo;

            _damageMultiplier = info.damageMultiplier;

            _superBulletDelay = info.superBulletInfo.superBulletDelay;
            _superBulletSpeed = info.superBulletInfo.superBulletSpeed;

            if (!_photonView.IsMine)
                return;

            //Chicken myChicken = GetComponent<Chicken>();
            //foreach (Chicken chicken in FindObjectsOfType<Chicken>())
            //    if (chicken.IsMine && chicken != myChicken)
            //        chicken.GetComponent<ItemPicker>().onStopingTime += CallStopTimeRPC;

            //foreach (ItemPicker picker in FindObjectsOfType<ItemPicker>())
            //    if (picker.enabled && picker != _itemPicker)
            //        picker.onStopingTime += CallStopTimeRPC;

            ChangeCombating((Combating)Random.Range(0, 3));
        }
        void OnGameEnded(EndStates end)
        {
            if (!_photonView.IsMine)
                return;
        }
        #endregion

        #region Updating
        protected override void Update()
        {
            base.Update();

            if (!_photonView.IsMine)
            {
                RefreshAimAngles();
                return;
            }

            CheckShooting();
        }
        #endregion

        #region Combating
        void CallUpdateCombatingRPC(Combating combat) => _photonView.RPC(nameof(UpdateCombatingRPC), RpcTarget.All, (int)_combat);
        [PunRPC]
        void UpdateCombatingRPC(int combat) => UpdateCombating((Combating)combat);
        #endregion

        #region Shooting
        void Attacking(Combating combat, float dmg, float speed, float angle = 0) => _photonView.RPC(nameof(AttackingRPC), RpcTarget.All,
                (int)combat, dmg, speed, _gunPosition, _gunRotaion + angle);
        [PunRPC]
        void AttackingRPC(int combat, float dmg, float speed, float[] position, float zRotation)
        {
            if ((Combating)combat != Combating.Super)
                ShootAttack(_photonView.IsMine, _myInfo.Index, combat, dmg, speed, position, zRotation);
            else
            {
                Explosion explosion = _fightManager.GetExplosion;
                ShootAttack(explosion, _photonView.IsMine, _myInfo.Index, combat, dmg, speed, position, zRotation);
            }
        }
        #endregion

        #region Cooldowns and timers
        void CallDoGunCooldown(float delay) => _photonView.RPC(nameof(DoGunCooldownRPC), RpcTarget.All, delay);
        [PunRPC]
        void DoGunCooldownRPC(float delay) => DoGunCooldown(delay);
        #endregion

        #region Pickup

        #region SuperBurger
        void CallBurgerBulletRPC(float duration) => _photonView.RPC(nameof(BurgerBulletRPC), RpcTarget.All, duration);
        [PunRPC]
        void BurgerBulletRPC(float duration) => BurgerBullet(duration);
        #endregion

        #region MiniGun
        void CallFireRateBoostRPC(float duration) => _photonView.RPC(nameof(FireRateBoostRPC), RpcTarget.All, duration);
        [PunRPC]
        void FireRateBoostRPC(float duration) => FireRateBoost(duration);
        #endregion

        #region Power
        void CallDoublePowerRPC(float duration) => _photonView.RPC(nameof(DoublePowerRPC), RpcTarget.All, duration);
        [PunRPC]
        void DoublePowerRPC(float duration) => DoublePower(duration);
        #endregion

        #region StopingTime
        void CallStopTimeRPC(float duration) => _photonView.RPC(nameof(StopTimeRPC), RpcTarget.All, duration);
        [PunRPC]
        void StopTimeRPC(float duration) => StopTime(duration);
        #endregion

        #endregion

        #region SyncRotation over PhotonStream (Observation)
        public void OnPhotonSerializeView(PhotonStream p_Stream, PhotonMessageInfo p_message)
        {
            if (p_Stream.IsWriting)
                p_Stream.SendNext((int)(gunAnchor.transform.localEulerAngles.z * 100f));
            else
                aimAngle = (int)p_Stream.ReceiveNext() / 100f;
        }

        void RefreshAimAngles()
        {
            float cacheEularX = gunAnchor.transform.localEulerAngles.x;
            Quaternion targetRotation = Quaternion.identity * Quaternion.AngleAxis(aimAngle, Vector3.forward);
            gunAnchor.rotation = Quaternion.Slerp(gunAnchor.rotation, targetRotation, Time.deltaTime * 8f);

            Vector3 finalRotation = gunAnchor.localEulerAngles;
            finalRotation.x = cacheEularX;
        }
        #endregion
    }
}