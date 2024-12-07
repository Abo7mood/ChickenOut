using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Items
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Items/JackBoxModeInfo", fileName = "New JackBoxModeInfo")]
    public class JackBoxModeInfo : ScriptableObject
    {
        [Min(1)] public int maxJackBoxesAllowed = 1;

        [Min(1)] public float jackChargeTime = 1;
        [Min(1)] public float jackSpeed = 1;

        public bool enableJack = true;
    }
}