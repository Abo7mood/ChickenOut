using UnityEngine;

namespace ChickenOut.Battle.Chickens.Movement
{
    internal class GroundCheck : MonoBehaviour
    {
        #region Variables
        ChickenMovementSystem _movementSystem;

        internal bool isGrounded;
        #endregion

        #region Starts
        void Awake() => _movementSystem = GetComponentInParent<ChickenMovementSystem>();
        #endregion

        #region Ground checking
        void OnTriggerEnter2D(Collider2D collision) => CheckGround(true);
        void OnTriggerExit2D(Collider2D collision) => CheckGround(false);

        void CheckGround(bool enter)
        {
            if (/*_movementSystem == null ||*/ (enter && _movementSystem.YVelocity > 0))
                return;

            if (enter && !isGrounded)
                _movementSystem.onLanded?.Invoke();

            isGrounded = enter;
        }
        #endregion
    }
}