using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ChickenOut.Battle;

namespace ChickenOut.Menus
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] Item item;
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI text;

        private void Start()
        {
            image.sprite = item.sprite;
            text.text = item.description;
        }
    }
}