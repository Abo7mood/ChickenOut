using UnityEngine;
using Photon.Pun;

namespace ChickenOut.Battle.Chickens.Movement
{
    public class VisualsController : MonoBehaviour
    {
        #region Varables
        ChickenMovementSystem _movementSystem;
        SpriteRenderer _spriteRenderer;
        PhotonView _photonView;
        #endregion

        #region Starts
        void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _movementSystem = GetComponentInParent<ChickenMovementSystem>();
            //if (_movementSystem != null)
                _movementSystem.onCheckFlipping += CheckFlipping;

            Chicken chicken = GetComponentInParent<Chicken>();
            if (chicken != null)
            {
                chicken.onTurning += gameObject.SetActive;
                chicken.onTeamingUp += TeamingUp;
            }
        }
        #endregion

        #region Flipping
        void CheckFlipping(float x)
        {
            if (x == 0 /*|| _spriteRenderer == null*/)
                return;

            if (ShouldFlipRight(x))
                CallFlipRPC(true);

            else if (ShouldFlipLeft(x))
                CallFlipRPC(false);
        }
        void CallFlipRPC(bool flip)
        {
            if (_photonView != null && _movementSystem != null && _movementSystem.IsOnline)
                _photonView.RPC(nameof(FlipRPC), RpcTarget.All, flip);
            else
                FlipRPC(flip);
        }
        [PunRPC]
        void FlipRPC(bool flip)
        {
            //if (_spriteRenderer != null)
                _spriteRenderer.transform.localScale = new Vector3(flip ? -1 : 1, 1, 1);
        }
        #endregion

        void TeamingUp(Color teamColor)
        {
            if (_spriteRenderer != null)
                _spriteRenderer.material.color = teamColor;
        }

        #region Bools
        bool ShouldFlipRight(float x) => x > 0 && _spriteRenderer.transform.localScale.x > 0;
        bool ShouldFlipLeft(float x) => x < 0 && _spriteRenderer.transform.localScale.x < 0;
        #endregion
    }
}