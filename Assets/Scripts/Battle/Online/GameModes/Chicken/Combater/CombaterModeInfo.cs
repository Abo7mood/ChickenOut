using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Chickens
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Chickens/Combater/CombaterModeInfo", fileName = "New CombaterModeInfo")]
    public class CombaterModeInfo : ScriptableObject
    {
        [Min(1)] public int damageMultiplier = 1;

        public SuperBulletModeInfo superBulletInfo;
    }
}