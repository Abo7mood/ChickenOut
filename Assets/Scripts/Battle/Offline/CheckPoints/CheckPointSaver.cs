using UnityEngine;

namespace ChickenOut.Battle.Offline.CheckPoints
{
    [CreateAssetMenu(menuName = "Mine/Item", fileName = "New Item")]
    public class CheckPointSaver : ScriptableObject
    {
        public int CheckPoint => _currentCheckPoint;
        int _currentCheckPoint = 0;

        public void SaveCheckPoint(int index) => _currentCheckPoint = index;
    }
}