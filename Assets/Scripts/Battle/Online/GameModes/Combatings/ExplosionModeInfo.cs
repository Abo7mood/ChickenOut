using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Combatings
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Combatings/ExplosionModeInfo", fileName = "New ExplosionModeInfo")]
    public class ExplosionModeInfo : ScriptableObject
    {
        [Min(1)] public float radius = 1;
        [Min(1)] public float growthSpeed = 1;
    }
}