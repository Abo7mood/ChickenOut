using UnityEngine;

namespace ChickenOut.Battle.Online.Items
{
    public class ItemPickupOnline : PickupOnline, IInteractable
    {
        #region Stuff
        ItemSpawnerOnline _spawner;

        int _index;
        float _chargeTime;
        #endregion

        #region Starting
        protected override void Start()
        {
            base.Start();

            _spawner = ItemSpawnerOnline.instance;
        }
        public void Setup(int index, float charge, Item item)
        {
            _index = index;
            _chargeTime = charge;
            _item = item;

            Setup();
        }
        #endregion

        #region Spawning
        public void Spawn(Vector2 position)
        {
            transform.position = position;
            Respawn();

            if (_chargeTime > 0)
                Invoke(nameof(TurnMeOff), _chargeTime);
        }
        #endregion

        #region Interacting
        public void Interact() => _spawner.CallInteractingRPC(_index);
        public void CallCollect() => Collect();
        #endregion
    }
}