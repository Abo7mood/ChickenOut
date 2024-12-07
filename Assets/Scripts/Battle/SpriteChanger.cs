using UnityEngine;
using UnityEngine.UI;
using ChickenOut.Battle.Managers;

namespace ChickenOut.Battle
{
    public class SpriteChanger : MonoBehaviour
    {
        Image myImage;
        Sprite originalSprite;

        HUDManager _myHUD;

        public bool IsOriginal => myImage.sprite == originalSprite;

        void Awake()
        {
            myImage = GetComponent<Image>();
            originalSprite = myImage.sprite;
        }
        void Start() => _myHUD = HUDManager.instance;

        public void CheckCombating(Combating combat)
        {
            if (combat == Combating.Super && IsOriginal)
                ChangeSprite(_myHUD._burgerSprite);

            else if (!IsOriginal)
                ChangeSprite();
        }
        public void ChangeSprite(Sprite newSprite) => myImage.sprite = newSprite;
        public void ChangeSprite() => myImage.sprite = originalSprite;
    }
}