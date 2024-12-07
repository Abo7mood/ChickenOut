using UnityEngine;

namespace ChickenOut.Battle.Offline.CheckPoints
{
    public class CheckPoint : MonoBehaviour
    {
        const string PLAYER = "Player";

        CheckPointsManager _checkPointsManager;

        int _index;

        public void Setup(CheckPointsManager manager, int index)
        {
            _checkPointsManager = manager;
            _index = index;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(PLAYER))
                _checkPointsManager.CheckPointReached(_index);
        }
    }
}