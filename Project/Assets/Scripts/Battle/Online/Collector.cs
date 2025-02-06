using UnityEngine;
using ChickenOut.Battle.Online.Modes;

namespace ChickenOut.Battle.Online
{   [CreateAssetMenu(menuName = "Mine/Collector", fileName = "Collector")]
    public class Collector : ScriptableObject
    {
        #region Properities
        public GameMode[] GameModes => _gameModes;
        #endregion

        #region Variables
        [SerializeField] GameMode[] _gameModes;
        #endregion
    }
}