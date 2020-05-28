using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuButton : MonoBehaviour, SwitchableUI
{
    public bool Is_Open = false;

    public BuildMenuBar ChildBar;

	#region UI 작용

	public void OnClick()
    {
        SoundManager.Instance.PlayEffect((int)SoundManager.Effect.MenuOpen);

        //  열기
        if (!Is_Open)
        {
            //  작용
            Is_Open = true;

            ChildBar.OpenBar();
			GameManager.Instance.RegisterUI(this);

			//  연출
			gameObject.SetActive(true);
            iTween.RotateTo(gameObject, iTween.Hash("z", -45.0f, "easetype", "easeOutQuart", "time", 0.3f, "ignoretimescale", true));
        }

        //  닫기
        else
        {
            //  작용
            Is_Open = false;
			GameManager.Instance.CancleUI(this);

			ChildBar.CloseBar();

            //  연출
            iTween.RotateTo(gameObject, iTween.Hash("z", 0.0f, "easetype", "easeInQuart", "time", 0.3f, "oncomplete", "DisableButton", "ignoretimescale", true));
        }
    }

    private void DisableButton()
    {
        gameObject.SetActive(false);
    }

	#endregion
}
