using UnityEngine;

namespace ChickenOut.Battle.Online.Modes.Match
{
    [CreateAssetMenu(menuName = "Mine/GameModes/Match/GunsModeInfo", fileName = "New GunsModeInfo")]
    public class GunsModeInfo : ScriptableObject
    {
        public Gun[] allGuns;
    }
}