using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButton : MonoBehaviour
{
	public bool Is_Open = false;

	public void OnClick()
	{
		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.MenuOpen);

		//  열기
		if (!Is_Open)
		{
			//  작용
			Is_Open = true;

			//  연출
			iTween.RotateTo(gameObject, iTween.Hash("z", -45.0f, "easetype", "easeOutQuart", "time", 0.3f, "ignoretimescale", true));
		}

		//  닫기
		else
		{
			//  작용
			Is_Open = false;

			//  연출
			iTween.RotateTo(gameObject, iTween.Hash("z", 0.0f, "easetype", "easeOutQuart", "time", 0.3f, "oncomplete", "DisableButton", "ignoretimescale", true));
		}
	}
}

