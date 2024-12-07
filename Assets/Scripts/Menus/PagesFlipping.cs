using UnityEngine;

namespace ChickenOut.Menus
{
    public class PagesFlipping : MonoBehaviour
    {
        #region Stuff
        [SerializeField] GameObject _rightButton, _leftButton, _pagesHolder;
        Page[] _pages;

        [SerializeField] [Min(1)] int _pagesPerFlip = 1;
        int _currentPage;

        int Max => _pages.Length / _pagesPerFlip;
        #endregion

        #region Starting
        void Awake() => _pages = _pagesHolder.GetComponentsInChildren<Page>(true);
        void Start() => _currentPage = 0;
        #endregion

        #region Other

        #region Buttons
        public void GoRight()
        {
            _currentPage.Next(Max);
            UpdatePages();
        }
        public void GoLeft()
        {
            _currentPage.Before(0);
            UpdatePages();
        }
        #endregion

        void UpdatePages()
        {
            for (int i = 0; i < Max; i++)
                for (int j = 0; j < _pagesPerFlip; j++)
                    _pages[(i * _pagesPerFlip) + j].gameObject.SetActive(i == _currentPage);

            _rightButton.SetActive(_currentPage < (Max - 1));
            _leftButton.SetActive(_currentPage > 0);
        }
        #endregion
    }
}