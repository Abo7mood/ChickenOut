using UnityEngine;

namespace ChickenOut.Battle.Chickens.Movement
{
    internal class AnimationController : MonoBehaviour
    {
        #region Variables
        const string SPEED = "Speed", IS_JUMPING = "IsJumping", IS_LANDING = "IsLanding", IS_DASHING = "IsDashing";

        ChickenMovementSystem _movementSystem;
        Animator _animator;
        #endregion

        #region Starts
        void Awake()
        {
            _movementSystem = GetComponentInParent<ChickenMovementSystem>();
            _animator = GetComponent<Animator>();
        }
        #endregion

        #region Updates
        void FixedUpdate()
        {
            if (/*_movementSystem == null ||*/ _movementSystem.CantUpdate)
                return;

            Animation();
        }
        #endregion

        #region Animation
        internal void Animation()
        {
            //if (_animator == null || _movementSystem == null)
            //    return;

            if (_movementSystem.IsGrounded)
            {
                _animator.SetFloat(SPEED, Mathf.Abs(_movementSystem.Movement.normalized.x));
                SetBools(false, false);
            }
            else
                SetBools(_movementSystem.YVelocity > 0, _movementSystem.YVelocity < -1f);
        }
        void SetBools(bool isJumping, bool isLanding)
        {
            _animator.SetBool(IS_JUMPING, isJumping);
            _animator.SetBool(IS_LANDING, isLanding);
        }
        #endregion
    }
}