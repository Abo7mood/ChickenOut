using UnityEngine;

namespace ChickenOut.Battle.Online.Items
{
    public class JackBoxPickupOnline : PickupOnline, IInteractable
    {
        #region Stuff
        JackSpawnerOnline _spawner;

        int _index;
        float _speed;
        Vector3 _direction;
        #endregion

        #region Starting
        protected override void Start()
        {
            base.Start();

            _spawner = JackSpawnerOnline.instance;
        }
        public void Setup(float speed, int index)
        {
            _speed = speed;
            _index = index;

            Setup();
        }
        #endregion

        #region Spawning + Moving
        public void SpawnJack(Vector2 position, Vector3 direction)
        {
            transform.position = position;
            _direction = direction;

            Respawn();
        }

        void Update() => transform.position += _direction * (_speed * Time.deltaTime);
        #endregion

        #region Interacting
        public void Interact() => _spawner.CallInteractingRPC(_index);
        public void CallCollect() => Collect();
        #endregion
    }
}