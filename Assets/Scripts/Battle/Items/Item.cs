using UnityEngine;

namespace ChickenOut.Battle
{
    [CreateAssetMenu(menuName = "Mine/Item", fileName = "New Item")]
    public class Item : ScriptableObject
    {
        public string myName;
        public string description;

        public float duration;
        public Sprite sprite;
        public ItemType type;
        public AnimatorOverrideController animator;
    }
}