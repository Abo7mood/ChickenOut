using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Chickens
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Chickens/Movement/MovementModeInfo", fileName = "New MovementModeInfo")]
    public class MovementModeInfo : ScriptableObject
    {
        [Min(1)] public int chickenSpeed = 1;

        public JumpModeInfo jumpInfo;
        public DashModeInfo dashInfo;
    }
}