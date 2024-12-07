using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changePages : MonoBehaviour
{
   public GameObject[] pages;
    int currentPage;

    public void GoRight()
    {
        currentPage++;
        if (currentPage > pages.Length - 1)
            currentPage = 0;

        UpdateUI();
    }

    public void GoLeft()
    {
        currentPage--;
        if (currentPage < 0)
            currentPage = pages.Length - 1;

        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].gameObject.SetActive(i == currentPage);
    }
}
