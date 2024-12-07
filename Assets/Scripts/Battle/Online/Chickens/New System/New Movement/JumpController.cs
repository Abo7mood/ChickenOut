using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Items;
using ChickenOut.Battle.Managers;

namespace ChickenOut.Battle.Chickens.Movement
{
    internal class JumpController : MonoBehaviour
    {
        #region Stuff

        #region Variables
        ChickenMovementSystem _movementSystem;
        SoundController _soundController;
        Rigidbody2D _rigidbody;
        PhotonView _photonView;

        float _jumpStrength = 10f;
        int _maxJumps = 1;
        int _timesJumped;
        bool _hasJumped;
        #endregion

        #region Properities
        internal bool CanJump => _timesJumped < _maxJumps && !_hasJumped;
        #endregion

        #endregion

        #region Starts
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _movementSystem = GetComponent<ChickenMovementSystem>();
            //if (_movementSystem != null)
                _movementSystem.onLanded += OnLanded;
        }
        internal void Setup(ItemPickingController pickingController)
        {
            if (/*pickingController != null && */pickingController.enabled)
                pickingController.onItemTypePickedup += ExtraJump;
        }
        #endregion

        #region Updates
        void Update()
        {
            if (/*_movementSystem == null ||*/ _movementSystem.CantUpdate)
                return;

            if (!_movementSystem.CantUpdate && _movementSystem.Movement.y < .5f)
                _hasJumped = false;
        }

        void FixedUpdate()
        {
            if (/*_movementSystem == null ||*/ _movementSystem.CantUpdate)
                return;

            Vector2 movement = _movementSystem.Movement;

            bool yCheck = (!_movementSystem.IsArrows && movement.y >= .9f) || (_movementSystem.IsArrows && movement.y > 0f);
            if (CanJump && _rigidbody.velocity.y <= 0 && yCheck)
                Jump();
        }
        #endregion

        void Jump()
        {
            CallJumpingEffectsRPC();
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpStrength);

            _timesJumped++;
            _hasJumped = true;

            //if (_movementSystem != null)
                _movementSystem.onJumped?.Invoke();
        }
        void OnLanded() => _timesJumped = 0;

        #region Remove to sounds controller
        void CallJumpingEffectsRPC()
        {
            //if (_movementSystem.IsOnline)
            //    _photonView.RPC(nameof(JumpingEffectsRPC), RpcTarget.All);
            //else
            //    JumpingEffectsRPC();
        }
        //[PunRPC] void JumpingEffectsRPC()
        //{
        //    if (_soundController != null)
        //        _soundController.JumpSound();
        //}
        #endregion

        #region Pickup
        void ExtraJump(ItemType type, float duration)
        {
            if (type != ItemType.ExtraJump)
                return;

            _maxJumps++;
            Invoke(nameof(RemoveExtraJump), duration);
        }
        void RemoveExtraJump() => _maxJumps--;
        #endregion
    }
}