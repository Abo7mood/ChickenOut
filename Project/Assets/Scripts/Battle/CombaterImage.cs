using UnityEngine;
using UnityEngine.UI;

namespace ChickenOut.Battle
{
    public class CombaterImage : MonoBehaviour
    {
        [SerializeField] Sprite[] _sprites = new Sprite[4];
        Image _myImage;

        void Awake() => _myImage = GetComponent<Image>();

        public void UpdateImage(int nm)
        {
            _myImage.sprite = _sprites[nm];
            _myImage.color = ((Combating)nm).GetColor();
        }
        public void UpdateImage(float nm) => _myImage.fillAmount = nm;
        public int GetImageSprite()
        {
            for (int i = 0; i < _sprites.Length; i++)
                if (_sprites[i] == _myImage.sprite)
                    return i;

            return -1;
        }
    }
}