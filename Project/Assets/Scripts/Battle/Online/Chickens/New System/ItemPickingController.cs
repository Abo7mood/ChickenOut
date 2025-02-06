using System;
using UnityEngine;
using Photon.Pun;
using ChickenOut.Battle.Managers;
using ChickenOut.Battle.Online.Items;

namespace ChickenOut.Battle.Chickens
{
    internal class ItemPickingController : MonoBehaviour
    {
        #region Stuff

        #region Events
        internal Action<Item> onItemPickedup;
        internal Action<ItemType, float> onItemTypePickedup;
        #endregion

        #region Properities
        internal bool IsOnline => PhotonNetwork.IsConnected && PhotonNetwork.InRoom;
        internal bool CantInteract => IsNotMine || _chicken.IsDead;
        internal bool IsMine => _photonView != null && _photonView.IsMine;
        internal bool IsNotMine => _photonView != null && !_photonView.IsMine;
        #endregion

        #region Variables
        private SoundController _soundManager;
        private PhotonView _photonView;
        private Chicken _chicken;

        private The_Timer_0 _pickupTimer;
        #endregion

        #endregion

        #region Starts
        void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _chicken = GetComponent<Chicken>();
        }
        void Start() => _soundManager = SoundController.instance;
        #endregion

        #region Pickup
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (CantInteract)
                return;

            IInteractable interact = collision.GetComponent<IInteractable>();
            if (interact != null && _pickupTimer.IsReady)
            {
                interact.Interact();

                PickupOnline pickup = collision.gameObject.GetComponent<PickupOnline>();
                if (pickup != null)
                    Pickup(pickup.Item);
            }
        }
        private void Pickup(Item item)
        {
            _pickupTimer.ResetartTimer(.01f);

            onItemTypePickedup?.Invoke(item.type, item.duration);
            onItemPickedup?.Invoke(item);
            CallPickUpEffectRPC();
        }
        #endregion

        #region Effects
        private void CallPickUpEffectRPC()
        {
            if (IsOnline)
                _photonView.RPC(nameof(PickUpEffectRPC), RpcTarget.All);
            else
                PickUpEffectRPC();
        }

        [PunRPC] private void PickUpEffectRPC()
        {
            if (_soundManager != null)
                _soundManager.AbilitySound();
        }
        #endregion
    }
}