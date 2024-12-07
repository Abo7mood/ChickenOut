using UnityEngine;

namespace ChickenOut.Battle.Chickens.Movement
{
    internal class MovementController : MonoBehaviour
    {
        #region Variables
        ChickenMovementSystem _movementSystem;
        Rigidbody2D _rigidbody;

        float _speed = 5f;
        #endregion

        #region Starts
        void Awake()
        {
            _movementSystem = GetComponent<ChickenMovementSystem>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        internal void Setup(ItemPickingController pickingController)
        {
            if (/*pickingController != null && */pickingController.enabled)
                pickingController.onItemTypePickedup += SpeedBoost;
        }
        #endregion

        #region Updates
        void FixedUpdate()
        {
            if (/*_movementSystem == null ||*/ _movementSystem.CantUpdate)
                return;

            if (!_movementSystem.IsDashing)
                Move(_movementSystem.Movement);
        }
        #endregion

        void Move(Vector2 movement)
        {
            _rigidbody.velocity = new Vector2(movement.normalized.x * _speed, _rigidbody.velocity.y);

            if (/*_movementSystem != null &&*/ movement.x != 0)
                _movementSystem.onCheckFlipping?.Invoke(movement.x);
        }

        #region Pcikup
        void SpeedBoost(ItemType type, float duration)
        {
            if (type != ItemType.SpeedBoost)
                return;

            _speed *= 2;
            Invoke(nameof(ResetSpeed), duration);
        }
        void ResetSpeed() => _speed /= 2;
        #endregion
    }
}