using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Chickens
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Chickens/ChickenModeInfo", fileName = "New ChickenModeInfo")]
    public class ChickenModeInfo : ScriptableObject
    {
        [Min(1)] public int startingPoints = 1;
        [Min(1)] public float maxHP = 1;
    }
}