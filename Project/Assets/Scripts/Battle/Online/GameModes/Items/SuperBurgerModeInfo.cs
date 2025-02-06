using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Items
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Items/SuperBurgerModeInfo", fileName = "New SuperBurgerModeInfo")]
    public class SuperBurgerModeInfo : ScriptableObject
    {
        [Min(1)] public float superBurgerChargeTime = 1;
        [Min(1)] public float superBurgerSpeed = 1;

        public bool enableBurger = true;
    }
}