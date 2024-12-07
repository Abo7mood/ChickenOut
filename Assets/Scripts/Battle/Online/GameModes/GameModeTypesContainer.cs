using UnityEngine;

namespace ChickenOut.Battle.Online.Modes
{
    [CreateAssetMenu(menuName = "Mine/GameModes/GameModeTypesContainer", fileName = "New GameModeTypesContainer")]
    public class GameModeTypesContainer : ScriptableObject
    {
        public GameModeType this[int index] => _types[index];

        public GameModeType[] Types => _types;

        [SerializeField] GameModeType[] _types;
    }
}