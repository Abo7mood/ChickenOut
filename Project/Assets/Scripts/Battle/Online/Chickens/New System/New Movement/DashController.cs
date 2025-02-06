using UnityEngine;

namespace ChickenOut.Battle.Chickens.Movement
{
    internal class DashController : MonoBehaviour
    {
        #region Stuff

        #region Variables
        ChickenMovementSystem _movementSystem;
        Rigidbody2D _rigidbody;

        float _dashForce = 15f;

        float _startDashTimer = .25f;
        float _dashChargeTime = 2f;
        float _currentdashtime;

        int _dashDirection;
        bool _hasDashed;
        bool _canDash = true;
        #endregion

        #region Properities
        internal bool CanDash => _canDash && !_hasDashed;
        internal bool IsDashing => _currentdashtime > 0;
        #endregion

        #endregion

        #region Starts
        void Awake()
        {
            _movementSystem = GetComponent<ChickenMovementSystem>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        #endregion

        #region Updates
        void Update()
        {
            if (/*_movementSystem == null ||*/ _movementSystem.CantUpdate)
                return;

            Vector2 movement = _movementSystem.Movement;
            float dasher = _movementSystem.Dasher;

            bool check = (_movementSystem.IsArrows && dasher != 0) || (!_movementSystem.IsArrows && Mathf.Abs(movement.x) > .95f);
            int dashDirection = (int)(_movementSystem.IsArrows ? dasher : (movement.x < 0 ? -1 : 1));

            if (CanDash && check)
                ShouldDash((int)dasher, dashDirection);

            if (!_movementSystem.CantUpdate && _movementSystem.Movement.x < .3f)
                _hasDashed = false;
        }

        void FixedUpdate()
        {
            if (/*_movementSystem == null ||*/ _movementSystem.CantUpdate)
                return;

            if (IsDashing)
                Dash();
        }
        #endregion

        #region Dash
        private void ShouldDash(int dasher, int direction)
        {
            _hasDashed = true;
            _canDash = false;

            _dashDirection = direction;
            Invoke(nameof(MakeCanDash), _dashChargeTime);

            //if (_movementSystem != null)
            {
                _movementSystem.onCheckFlipping?.Invoke(_dashDirection);
                _movementSystem.onDashed?.Invoke(_dashDirection, _startDashTimer);
            }

            _currentdashtime = _startDashTimer;
            _rigidbody.velocity = Vector2.zero;
        }
        internal void Dash()
        {
            _rigidbody.velocity = transform.right * (_dashForce * _dashDirection);
            _currentdashtime -= Time.deltaTime;
        }
        void MakeCanDash() => _canDash = true;
        #endregion

        #region Remove to sound controller
        //void CallDashingEffectsRPC(int direction, float delay)
        //{
        //    if (_movementSystem.IsOnline)
        //        _photonView.RPC(nameof(DashingEffectsRPC), RpcTarget.All, direction, _photonView.IsMine);
        //    else
        //        DashingEffectsRPC(direction);
        //}
        //[PunRPC]
        //void DashingEffectsRPC(int direction, bool something = true)
        //{
        //    _soundController.DashSound();
        //    //if (something)
        //    //    _animator.SetTrigger(IS_DASHING);
        //}
        #endregion
    }
}