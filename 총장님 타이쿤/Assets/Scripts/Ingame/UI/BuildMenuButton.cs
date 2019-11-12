using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuButton : MonoBehaviour
{
    public bool Is_Open = false;

    public BuildMenuBar ChildBar;


    public void OnClick()
    {
        //  열기
        if (!Is_Open)
        {
            //  작용
            ChildBar.OpenBar();

            //  연출
            iTween.RotateTo(gameObject, iTween.Hash("z", -45.0f, "easetype", "easeOutQuart", "time", 0.3f));

            Is_Open = true;
        }

        //  닫기
        else
        {
            //  작용
            ChildBar.CloseBar();

            //  연출
            iTween.RotateTo(gameObject, iTween.Hash("z", 0.0f, "easetype", "easeInQuart", "time", 0.3f));

            Is_Open = false;
        }
    }
}
