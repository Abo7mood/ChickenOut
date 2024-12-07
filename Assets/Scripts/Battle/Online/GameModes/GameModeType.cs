using UnityEngine;

namespace ChickenOut.Battle.Online.Modes
{
    [CreateAssetMenu(menuName = "Mine/GameModes/GameModeType", fileName = "New GameModeType")]
    public class GameModeType : ScriptableObject
    {
        #region Properities
        public string TypeName
        {
            get
            {
                if (IsTeam)
                    return $"({MaxPlayersPerTeam}v{MaxPlayersPerTeam})";
                else
                    return $"[{MaxPlayers}]";
            }
        }

        public int MaxPlayers => _maxPlayers;
        public int MaxPlayersPerTeam => _maxPlayers / 2;

        public bool IsTeam => _isTeam;
        #endregion

        [SerializeField, Range(1, 4)] int _maxPlayers = 1;

        [SerializeField] bool _isTeam;
    }
}