using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Image))]
public class TapButton : MonoBehaviour ,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
    public TapGroup tapGroup;
    public Image backGround;

    public void OnPointerClick(PointerEventData eventData)
    {
        tapGroup.OnSelectButton(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tapGroup.OnEnterButton(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tapGroup.OnExitButton(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        backGround = GetComponent<Image>();
        tapGroup.subscription(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
