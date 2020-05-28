using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuBar : MonoBehaviour
{
    public BuildingCategoryButton OpenedButton;

	#region UI 연출

	public void OpenBar()
    {
        gameObject.SetActive(true);

        iTween.MoveTo(gameObject, iTween.Hash("x", 0.0f, "islocal", true, "easetype", "easeInQuart", "time", 0.3f, "ignoretimescale", true));
    }

    public void CloseBar()
    {
        //  패널 닫기
        if (OpenedButton)
            OpenedButton.ClosePanel();

        iTween.MoveTo(gameObject, iTween.Hash("x", -800.0f, "islocal", true, "easetype", "easeInQuart", "time", 0.3f, "oncomplete", "DisableBar", "ignoretimescale", true));
    }

    public void DisableBar()
    {
        gameObject.SetActive(false);
    }

	#endregion
}
