using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Chickens
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Chickens/Combater/SuperBulletModeInfo", fileName = "New SuperBulletModeInfo")]
    public class SuperBulletModeInfo : ScriptableObject
    {
        [Min(.1f)] public float superBulletDelay = .1f;
        [Min(.1f)] public float superBulletSpeed = .1f;
    }
}