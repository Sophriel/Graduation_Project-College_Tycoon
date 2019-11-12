﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuBar : MonoBehaviour
{
    public BuildingCategoryButton OpenedButton;

    public void OpenBar()
    {
        gameObject.SetActive(true);

        iTween.MoveTo(gameObject, iTween.Hash("x", 0.0f, "islocal", true, "easetype", "easeInQuart", "time", 0.3f));
    }

    public void CloseBar()
    {
        //  패널 닫기
        if (OpenedButton)
            OpenedButton.ClosePanel();

        iTween.MoveTo(gameObject, iTween.Hash("x", -800.0f, "islocal", true, "easetype", "easeInQuart", "time", 0.3f, "oncomplete", "DisableBar"));
    }

    public void DisableBar()
    {
        gameObject.SetActive(false);
    }
}
