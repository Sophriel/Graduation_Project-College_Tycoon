using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMenu : MonoBehaviour, SwitchableUI
{
	#region 메뉴 자체의 기능

	public bool Is_Open = false;

	public void OnClick()
	{
		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.MenuOpen);

		//  열기
		if (!Is_Open)
		{
			OpenMainMenu();
		}

		//  닫기
		else
		{
			CloseMainMenu();
		}
	}

	public void OpenMainMenu()
	{
		//  작용
		Is_Open = true;

		GameManager.Instance.RegisterUI(this);
		ScheduleManager.Instance.GameTime.OnPauseButton();

		//  연출
		gameObject.SetActive(true);
		iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "easetype", "easeOutQuart", "time", 0.3f, "ignoretimescale", true));
	}

	public void CloseMainMenu()
	{
		//  작용
		Is_Open = false;
		GameManager.Instance.CancleUI(this);
		ScheduleManager.Instance.GameTime.OnPlayButton();

		//  연출
		iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "easetype", "easeInQuart", "time", 0.3f, "oncomplete", "DisableMenu", "ignoretimescale", true));
	}

	private void DisableMenu()
	{
		gameObject.SetActive(false);
	}

	#endregion

	#region 메뉴 내부의 기능

	public void MoveToMainMenu()
	{
		ScheduleManager.Instance.GameTime.OnPlayButton();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
	}

	#endregion
}
