using System;
using UnityEngine;

namespace ChickenOut.Battle.Chickens
{
    internal class GroundCheck : MonoBehaviour
    {
        #region Stuff
        internal Action<bool> onGrounded;

        ChickenMovement _movement;
        #endregion

        #region Starts
        void Awake() => _movement = GetComponentInParent<ChickenMovement>();
        #endregion

        #region Ground checking
        void OnTriggerStay2D(Collider2D collision)
        {
            if (!_movement.isGrounded)
                onGrounded?.Invoke(true);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (_movement.isGrounded)
                onGrounded?.Invoke(false);
        }
        #endregion
    }
}