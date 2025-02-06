using UnityEngine;
using ChickenOut.Battle.Chickens;

namespace ChickenOut.Battle
{
    public class ChickenAnimator : MonoBehaviour
    {
        #region Stuff
        SpriteRenderer _spriteRenderer;
        #endregion

        #region Starting
        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            Chicken chicken = GetComponentInParent<Chicken>();
            chicken.onTurning += gameObject.SetActive;
            chicken.onTeamingUp += TeamingUp;

            ChickenMovement movement = GetComponentInParent<ChickenMovement>();
            movement.onFlipped += Flipping;
        }
        #endregion

        #region Other
        void Flipping(bool flip) => transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
        void TeamingUp(Color teamColor) => _spriteRenderer.material.color = teamColor;
        #endregion
    }
}