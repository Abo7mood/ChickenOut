using UnityEngine;

namespace ChickenOut.Battle.Offline.Enemies
{
    public class Enemy : Unit
    {
        EnemyMovement controller;
        EnemyCombater combater;

        public override void Setup()
        {
            base.Setup();

            controller = GetComponent<EnemyMovement>();
            combater = GetComponentInChildren<EnemyCombater>();

            //combater.Attacking();
            controller.Setup(this);
        }

        public override void Die()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }
}
