using System;
using UnityEngine;
using ChickenOut.Battle.Managers;

namespace ChickenOut.Battle.Items
{
    public class ItemPicker : MonoBehaviour
    {
        #region Stuff
        public Action<Item> onItemCollected;
        public Action onPickupEffect;

        public Action<float> onGetInBoxCalled;
        public Action<float> onHealCollected;

        public Action<float> onSpeedBoosted;
        public Action<float> onExtraJumped;

        public Action<float> onFireRateBoostCollected;
        public Action<float> onSuperBurgerCollected;
        public Action<float> onPowerCollected;
        public Action<float> onStopingTime;

        SoundController _soundManager;
        protected The_Timer_0 _pickupTimer;
        #endregion

        #region Starting
        void Start() => _soundManager = SoundController.instance;
        #endregion

        #region Pickup
        protected void PickingUp(Item item)
        {
            Pickup(item);
            _pickupTimer = new The_Timer_0(.01f);

            onItemCollected?.Invoke(item);
            onPickupEffect?.Invoke();
        }
        void Pickup(Item item)
        {
            switch (item.type)
            {
                #region Chicken
                case ItemType.Heart:
                    onHealCollected?.Invoke(item.duration);
                    break;

                case ItemType.Jack:
                    onGetInBoxCalled?.Invoke(item.duration);
                    break;
                    #endregion

                #region Combater
                case ItemType.FireRateBoost:
                    onFireRateBoostCollected?.Invoke(item.duration);
                    break;

                case ItemType.SuperBurger:
                    onSuperBurgerCollected?.Invoke(item.duration);
                    break;

                case ItemType.Power:
                    onPowerCollected?.Invoke(item.duration);
                    break;

                case ItemType.StopingTime:
                    onStopingTime?.Invoke(item.duration);
                    break;
                #endregion

                #region Movement
                case ItemType.ExtraJump:
                    onExtraJumped?.Invoke(item.duration);
                    break;

                case ItemType.SpeedBoost:
                    onSpeedBoosted?.Invoke(item.duration);
                    break;
                #endregion
            }
        }
        protected void PickUpEffect()
        {
            if (_soundManager != null)
                _soundManager.AbilitySound();
        }
        #endregion
    }
}