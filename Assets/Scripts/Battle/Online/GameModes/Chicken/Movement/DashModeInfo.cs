using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Chickens
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Chickens/Movement/DashModeInfo", fileName = "New DashModeInfo")]
    public class DashModeInfo : ScriptableObject
    {
        [Min(1)] public int dashChargeTime = 1;
        [Min(1)] public int dashForce = 1;

        [Min(.1f)] public float startDashTimer = .1f;
    }
}