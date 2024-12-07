using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using ChickenOut.Battle.Items;
using ChickenOut.Battle.Managers;
using ChickenOut.Battle.Online.Modes;
using ChickenOut.Battle.Online.Modes.Chickens;

namespace ChickenOut.Battle.Chickens.Movement
{
    public class ChickenMovementSystem : MonoBehaviour
    {
        #region Stuff

        #region Events
        internal delegate void OnCheckFlipping(float x);
        internal OnCheckFlipping onCheckFlipping;

        internal delegate void OnDashed(int direction, float duration);
        internal OnDashed onDashed;

        internal delegate void OnJumped();
        internal OnJumped onJumped;

        internal delegate void OnLanded();
        internal OnLanded onLanded;
        #endregion

        #region Properities
        internal bool IsDashing => _dashController.IsDashing;
        internal bool IsGrounded => _groundCheck.isGrounded;
        internal float YVelocity => _rigidBody.velocity.y;
        internal bool IsArrows=> _isArrows;

        internal Vector2 Movement => _moveAction.ReadValue<Vector2>();
        internal float Dasher => _dashAction.ReadValue<float>();

        internal bool CantUpdate => IsInputNull || (IsOnline && !_photonView.IsMine);
        internal bool IsOnline => PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
        bool IsInputNull => _moveAction == null || _dashAction == null;
        #endregion

        #region Variables

        #region Inputs
        InputAction _moveAction;
        InputAction _dashAction;
        #endregion

        #region Controllers
        DashController _dashController;
        GroundCheck _groundCheck;
        #endregion

        #region Other
        Rigidbody2D _rigidBody;
        PhotonView _photonView;
        ChickenInfo _myInfo;
        Chicken _chicken;

        bool _isArrows = true;
        #endregion

        #endregion

        #endregion

        #region Starts
        void Awake()
        {
            _groundCheck = GetComponentInChildren<GroundCheck>();
            _dashController = GetComponent<DashController>();
            _photonView = GetComponent<PhotonView>();
            _rigidBody = GetComponent<Rigidbody2D>();
            _chicken = GetComponent<Chicken>();

            ItemPickingController pickingController = GetComponent<ItemPickingController>();
            //if (picker != null)
            {
                MovementController movementController = GetComponent<MovementController>();
                //if (movementController != null)
                    movementController.Setup(pickingController);

                JumpController jumpController = GetComponent<JumpController>();
                //if (jumpController != null)
                    jumpController.Setup(pickingController);
            }
        }
        void Start()
        {
            DoControls(GetComponent<PlayerInput>()); // for now

            //if (GameManagerOnline.instance != null)
            //{
            //    GameManagerOnline _gameManager = GameManagerOnline.instance;
            //    _gameManager.onGamePrepared += OnGamePrepared;
            //    _gameManager.onGameStarted += OnGameStarted;
            //    _gameManager.onGameEnded += OnGameEnded;
            //}
        }

        public void DoControls(PlayerInput input)
        {
            _moveAction = input.actions["Move"];
            _dashAction = input.actions["Dash"];
        }
        public void GetInfo(ChickenInfo info) => _myInfo = info;

        //#region Game state managing
        //void OnGamePrepared(GameMode mode)
        //{
        //    MovementModeInfo info = mode.MovementInfo;

        //    _speed = info.chickenSpeed;

        //    _jumpStrength = info.jumpInfo.jumpStrength;
        //    _maxJumps = info.jumpInfo.maxJumps;

        //    _dashForce = info.dashInfo.dashForce;
        //    _dashChargeTime = info.dashInfo.dashChargeTime;
        //    _startDashTimer = info.dashInfo.startDashTimer;
        //}
        //void OnGameStarted()
        //{
        //    if (!_photonView.IsMine)
        //        return;

        //    if (Application.isMobilePlatform || Application.isConsolePlatform)
        //        _isArrows = _chicken.Info.leftControls == LSControls.Arrows;
        //    else
        //        _isArrows = true;

        //    _rigidBody.bodyType = RigidbodyType2D.Dynamic;

        //    _canDash = true;
        //}
        //void OnGameEnded(EndStates end)
        //{
        //    if (!_photonView.IsMine)
        //        return;

        //    _rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        //}
        //#endregion

        #endregion
    }
}