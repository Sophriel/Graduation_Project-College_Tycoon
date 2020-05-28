using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCategoryButton : MonoBehaviour
{
    public bool Is_Open = false;

    public BuildMenuBar ParentBar;
    public GameObject ChildPanel;

    private void Start()
    {
        ParentBar = GetComponentInParent<BuildMenuBar>();
    }

	#region OnClick 구현

	public void OnClick()
    {
        SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Click);

        //  열기
        if (!Is_Open)
        {
            //  작용
            OpenPanel();
        }

        //  닫기
        else
        {
            //  작용
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        //  이미 열린 패널이 있는 경우
        if (ParentBar.OpenedButton)
            ParentBar.OpenedButton.ClosePanel();
        
        ChildPanel.SetActive(true);

        Is_Open = true;

        ParentBar.OpenedButton = this;
    }

    public void ClosePanel()
    {
        ChildPanel.SetActive(false);

        Is_Open = false;

        ParentBar.OpenedButton = null;
    }

	#endregion
}
