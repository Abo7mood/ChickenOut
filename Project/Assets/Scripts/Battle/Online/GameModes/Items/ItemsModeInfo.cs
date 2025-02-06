using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Items
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Items/ItemsModeInfo", fileName = "New ItemsModeInfo")]
    public class ItemsModeInfo : ScriptableObject
    {
        public Item[] items;

        [Min(1)] public float itemDisappearTime = 1;
        [Min(1)] public float itemChargeTime = 1;

        public bool enableItems = true;
    }
}