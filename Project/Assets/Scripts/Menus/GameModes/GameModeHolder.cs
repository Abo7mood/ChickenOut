using UnityEngine;
using TMPro;
using ChickenOut.Battle.Online.Modes;

namespace ChickenOut.Menus
{
    public class GameModeHolder : MonoBehaviour
    {
        #region Events
        public delegate void OnTypeChanged(int typeIndex);
        public OnTypeChanged onTypeChanged;
        #endregion

        #region Variables
        GameMode _myMode;
        int _myIndex;

        [SerializeField] Transform _holder;
        [SerializeField] GameModeTypeHolder _GameModeTypeButtonPrefab;

        [SerializeField] TMP_Text _titleText, _descriptionText;
        #endregion

        #region Starting
        public void Setup(GameMode mode, int index)
        {
            _myMode = mode;
            _myIndex = index;

            SetupTypes();
            UpdateUI();

            onTypeChanged += ChangeType;
        }

        void SetupTypes()
        {
            for (int i = 0; i < _myMode.MyTypes.Length; i++)
                Instantiate(_GameModeTypeButtonPrefab, _holder).Setup(this, _myMode.MyTypes[i], i);
        }
        void UpdateUI()
        {
            _titleText.text = _myMode.MyName;
            _descriptionText.text = _myMode.MyDescription;
        }
        #endregion

        #region Other
        public void PlayButton() => Launcher.instance.onPlayButtonWasPressed?.Invoke(_myIndex);

        public void ChangeType(int typeIndex)
        {
            _myMode.currentType = typeIndex;
            UpdateUI();
        }
        #endregion
    }
}