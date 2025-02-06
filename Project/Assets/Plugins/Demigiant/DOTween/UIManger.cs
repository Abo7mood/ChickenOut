using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManger : MonoBehaviour
{
    public RectTransform imge;
    void Start()
    {
         imge.DOAnchorPos(new Vector2(0, -10),0.25f);
    }

    void Update()
    {

    }
}
