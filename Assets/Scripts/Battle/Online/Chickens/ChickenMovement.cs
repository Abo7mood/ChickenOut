using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Chickens;

namespace ChickenOut.Battle.Online.Chickens
{
    public class ChickenMovement : Battle.Chickens.ChickenMovement
    {
        #region Stuff
        PhotonView _photonView;
        Chicken _chicken;
        #endregion

        #region Starting
        protected override void Awake()
        {
            base.Awake();

            _photonView = GetComponent<PhotonView>();
            _chicken = GetComponent<Chicken>();
        }
        protected override void Start()
        {
            base.Start();

            GameManagerOnline _gameManager = GameManagerOnline.instance;
            _gameManager.onGamePrepared += OnGamePrepared;
            _gameManager.onGameStarted += OnGameStarted;
            _gameManager.onGameEnded += OnGameEnded;

            onJumped += CallJumpingEffectsRPC;
            onDashed += CallDashingEffectsRPC;

            CallRPCFlipping += CallFlipRPC;
        }

        void OnGamePrepared(GameMode mode)
        {
            MovementModeInfo info = mode.MovementInfo;

            _speed = info.chickenSpeed;

            _jumpStrength = info.jumpInfo.jumpStrength;
            _maxJumps = info.jumpInfo.maxJumps;

            _dashForce = info.dashInfo.dashForce;
            _dashChargeTime = info.dashInfo.dashChargeTime;
            _startDashTimer = info.dashInfo.startDashTimer;
        }
        void OnGameStarted()
        {
            if (!_photonView.IsMine)
                return;

            if (Application.isMobilePlatform || Application.isConsolePlatform)
                _isArrows = _chicken.Info.leftControls == LSControls.Arrows;
            else
                _isArrows = true;

            rigidBody.bodyType = RigidbodyType2D.Dynamic;

            _canDash = true;
        }
        void OnGameEnded(EndStates end)
        {
            if (!_photonView.IsMine)
                return;

            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        #endregion

        #region
        void Update() => UpdateFrame(_photonView.IsMine);
        void FixedUpdate() => FixedUpdateFrame(_photonView.IsMine);

        void CallFlipRPC(bool flip) => _photonView.RPC(nameof(FlipRPC), RpcTarget.Others, flip);
        [PunRPC] void FlipRPC(bool flip) => Flip(flip);
        #endregion

        #region Effects
        void CallJumpingEffectsRPC() => _photonView.RPC(nameof(JumpingEffectsRPC), RpcTarget.All);
        [PunRPC] void JumpingEffectsRPC() => JumpingEffects();

        void CallDashingEffectsRPC(int direction, float delay) => _photonView.RPC(nameof(DashingEffectsRPC), RpcTarget.All, direction);
        [PunRPC] void DashingEffectsRPC(int direction) => DashingEffects(direction, _photonView.IsMine);
        #endregion
    }
}