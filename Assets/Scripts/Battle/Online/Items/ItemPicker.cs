using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Online.Chickens;

namespace ChickenOut.Battle.Online.Items
{
    public class ItemPicker : Battle.Items.ItemPicker
    {
        #region Stuff
        PhotonView _photonView;
        Chicken _chicken;
        #endregion

        #region Starting
        void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _chicken = GetComponent<Chicken>();
        }
        void Start() => onPickupEffect += CallPickUpEffectRPC;
        #endregion

        #region Updating
        void OnTriggerStay2D(Collider2D collision)
        {
            if (!_photonView.IsMine || _chicken.IsDead)
                return;

            IInteractable interact = collision.GetComponent<IInteractable>();
            if (interact != null && _pickupTimer.IsReady)
            {
                interact.Interact();

                PickupOnline pickup = collision.gameObject.GetComponent<PickupOnline>();
                if (pickup != null)
                    PickingUp(pickup.Item);
            }
        }
        #endregion

        #region Other
        void CallPickUpEffectRPC() => _photonView.RPC(nameof(PickUpEffectRPC), RpcTarget.All);
        [PunRPC]
        void PickUpEffectRPC() => PickUpEffect();
        #endregion
    }
}