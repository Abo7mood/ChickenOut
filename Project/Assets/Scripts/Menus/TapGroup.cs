using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapGroup : MonoBehaviour
{
    public List<TapButton> tapButtons;
    public Sprite tapidle;
    public TapButton selectTap;
    public List<GameObject> ObjectsToSwap;
    public void subscription(TapButton button)
    {
        if (tapButtons == null)
        {
            tapButtons = new List<TapButton>();
        }
        tapButtons.Add(button);
    }
    public void OnEnterButton(TapButton button)
    {
        ResetTabs();
        if (selectTap == null || button != selectTap)
        {
        }
       
    }
    public void OnExitButton(TapButton button)
    {
        ResetTabs();
    }  public void OnSelectButton(TapButton button)
    {
        ResetTabs();
        selectTap = button;
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < ObjectsToSwap.Count; i++)
        {
            if (i == index)
            {
             ObjectsToSwap[i].SetActive(true);
            }
            else
            {
            ObjectsToSwap[i].SetActive(false);
            }  
        }
      
    }
    public void ResetTabs()
    {
        foreach(TapButton button in tapButtons)
        {
            if(selectTap!=null&&button == selectTap) { continue; }
            button.backGround.sprite = tapidle;
        }
    }
}
