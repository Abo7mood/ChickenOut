using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Chickens
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Chickens/Movement/JumpModeInfo", fileName = "New JumpModeInfo")]
    public class JumpModeInfo : ScriptableObject
    {
        [Min(1)] public int jumpStrength = 1;
        [Min(1)] public int maxJumps = 1;
    }
}