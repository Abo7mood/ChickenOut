using UnityEngine;
using ChickenOut.Battle.Online.Modes;
using TMPro;

namespace ChickenOut.Menus
{
    public class GameModeTypeHolder : MonoBehaviour
    {
        #region Variables
        GameModeHolder _myHolder;

        GameModeType _myType;
        int _myIndex;

        [SerializeField] TMP_Text _titleText;
        #endregion

        #region Starting
        public void Setup(GameModeHolder holder, GameModeType type, int index)
        {
            _myHolder = holder;

            _myType = type;
            _myIndex = index;

            UpdateUI();
        }

        void UpdateUI() => _titleText.text = _myType.TypeName;
        #endregion

        #region Other
        public void ChooseType() => _myHolder.onTypeChanged?.Invoke(_myIndex);
        #endregion
    }
}