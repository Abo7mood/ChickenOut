using UnityEngine;

namespace ChickenOut.Battle.Offline.CheckPoints
{
    public class CheckPointsManager : MonoBehaviour
    {
        public static CheckPointsManager instance;

        [SerializeField] CheckPointSaver _saver;
        [SerializeField] CheckPoint[] _checkPoints;

        public Vector2 CheckPointPosition => _checkPoints[_saver.CheckPoint].transform.position;

        void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;
        }
        void Start()
        {
            for (int i = 0; i < _checkPoints.Length; i++)
                _checkPoints[i].Setup(this, i);
        }

        public void CheckPointReached(int index) => _saver.SaveCheckPoint(index);
    }
}