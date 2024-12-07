using UnityEngine;

namespace ChickenOut.Battle
{
    public class Spawner : MonoBehaviour
    {
        protected The_Timer_0 spawnTimer;

        protected virtual void ResetTimer(float newChargeTime = 0)
        {
            spawnTimer.SetTime((newChargeTime != 0) ? newChargeTime : spawnTimer.OriginalChargeTime);
            spawnTimer.ResetartTimer();
        }
    }
}