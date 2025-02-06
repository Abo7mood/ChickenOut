using UnityEngine;
using ChickenOut.Battle.Online.Modes.Match;
using ChickenOut.Battle.Online.Modes.Items;
using ChickenOut.Battle.Online.Modes.Chickens;
using ChickenOut.Battle.Online.Modes.Combatings;

namespace ChickenOut.Battle.Online.Modes
{
    [CreateAssetMenu(menuName = "Mine/GameModes/GameMode", fileName = "New GameMode")]
    public class GameMode : ScriptableObject
    {
        #region Properities
        public string MyName => $"{_myName} {MyType.TypeName}";
        public string MyDescription => $"{_myName} {MyType.TypeName} {_myDescription}";

        public Sprite MySprite => _mySprite;

        public GameModeType MyType => _myTypes[currentType];
        public GameModeType[] MyTypes => _myTypes.Types;

        #region Infos
        public GunsModeInfo GunsInfo => _gunsInfo;
        public TimersModeInfo TimersInfo => _timersInfo;

        public ItemsModeInfo ItemsInfo => _itemsInfo;
        public JackBoxModeInfo JackBoxInfo => _jackBoxInfo;
        public SuperBurgerModeInfo SuperBurgerInfo => _superBurgerInfo;

        public ExplosionModeInfo ExplosionInfo => _explosionInfo;

        public ChickenModeInfo ChickenInfo => _chickenInfo;
        public MovementModeInfo MovementInfo => _movementInfo;
        public CombaterModeInfo CombaterInfo => _combaterInfo;
        #endregion

        #endregion

        [SerializeField] string _myName;
        [SerializeField, TextArea] string _myDescription;

        [SerializeField] Sprite _mySprite;

        public int currentType;

        [SerializeField] GameModeTypesContainer _myTypes;

        #region Infos
        [Header("Match")]
        [SerializeField] GunsModeInfo _gunsInfo;
        [SerializeField] TimersModeInfo _timersInfo;

        [Header("Items")]
        [SerializeField] ItemsModeInfo _itemsInfo;
        [SerializeField] JackBoxModeInfo _jackBoxInfo;
        [SerializeField] SuperBurgerModeInfo _superBurgerInfo;

        [Header("Combatings")]
        [SerializeField] ExplosionModeInfo _explosionInfo;

        [Header("Chicken")]
        [SerializeField] ChickenModeInfo _chickenInfo;
        [SerializeField] MovementModeInfo _movementInfo;
        [SerializeField] CombaterModeInfo _combaterInfo;
        #endregion
    }
}