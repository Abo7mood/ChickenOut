using UnityEngine;

namespace ChickenOut.Battle.Online.Items
{
    public class SuperBurgerPickupOnline : PickupOnline, IInteractable
    {
        SuperBurgerSpawnerOnline _spawner;

        protected override void Start()
        {
            base.Start();

            _spawner = SuperBurgerSpawnerOnline.instance;
            Setup();
        }

        public void SpawnSuperBurger(Vector2 position)
        {
            transform.position = position;
            Respawn();
        }

        public void Interact() => _spawner.CallInteractingRPC();
        public void CallCollect() => Collect();
    }
}