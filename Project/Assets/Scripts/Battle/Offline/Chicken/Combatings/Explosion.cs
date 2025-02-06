using UnityEngine;

namespace ChickenOut.Battle.Offline.Combatings.Offence
{
    public class Explosion : Battle.Combatings.Offence.Explosion
    {
        [SerializeField] float __radius = 7f, __growthSpeed = 5f;

        protected override void Start()
        {
            base.Start();

            _radius = __radius;
            _growthSpeed = __growthSpeed;
        }
    }
}