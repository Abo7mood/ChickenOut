using UnityEngine;
using ChickenOut.Battle.Managers;

namespace ChickenOut.Battle.Offline.Managers
{
    public class FightManagerOffline : FightManager
    {
        public static FightManagerOffline instance;

        [SerializeField] new Gun[] __allGuns;

        void Awake()
        {
            if (instance != null)
                Debug.LogError($"More than one {instance} found", this);
            instance = this;

            _allGuns = __allGuns;
        }
    }
}