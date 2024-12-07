using UnityEngine;
using ChickenOut.Battle.Items;
using ChickenOut.Battle.Chickens;

namespace ChickenOut.Battle
{
    public class ParticlesController : MonoBehaviour
    {
        #region Stuff
        [SerializeField] ParticleSystem _startsPS, _healPS, _FeathersPS, _fireRateBoost, _power, _extraJumpPS, _speedBoostPS;
        [SerializeField] ParticleSystem _jumpPS, _landPS, _dashPS;
        #endregion

        #region Starting
        protected virtual void Awake()
        {
            ChickenMovement movement = GetComponent<ChickenMovement>();
            movement.onJumped += JumpParticle;
            movement.onLanded += LandParticle;
            movement.onDashed += DashParticle;

            GetComponent<ItemPicker>().onItemCollected += CallItemParticle;
            GetComponent<Chicken>().onDamageTaken += FeathersParticle;
        }
        #endregion

        #region Other
        void CallItemParticle(Item item) => ItemParticle(item.type);
        protected void ItemParticle(ItemType type)
        {
            _startsPS.Play();

            switch (type)
            {
                case ItemType.Heart:
                    _healPS.Play();
                    break;

                case ItemType.ExtraJump:
                    _extraJumpPS.Play();
                    break;

                case ItemType.SpeedBoost:
                    _speedBoostPS.Play();
                    break;

                case ItemType.FireRateBoost:
                    _fireRateBoost.Play();
                    break;

                case ItemType.Power:
                    _power.Play();
                    break;
            }
        }

        protected void FeathersParticle(float nm) => _FeathersPS.Play();
        #endregion

        #region Movement
        protected void JumpParticle() => _jumpPS.Play();
        protected void LandParticle() => _landPS.Play();
        protected void DashParticle(int dashDirection, float delay)
        {
            int y = dashDirection > 0 ? -90 : dashDirection < 0 ? 90 : 0;
            _dashPS.gameObject.transform.eulerAngles = new Vector3(0, y);
            _dashPS.Play();

            Invoke(nameof(StopDashing), delay);
        }
        void StopDashing() => _dashPS.Stop();
        #endregion
    }
}