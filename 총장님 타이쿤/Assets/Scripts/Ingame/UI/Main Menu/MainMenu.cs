using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour, SwitchableUI
{
	private void Start()
	{
		transform.localScale = Vector3.zero;
	}

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

		//  연출
		gameObject.SetActive(true);
		iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "easetype", "easeOutQuart", "time", 0.3f, "ignoretimescale", true));
	}

	public void CloseMainMenu()
	{
		//  작용
		Is_Open = false;
		GameManager.Instance.CancleUI(this);

		//  연출
		iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "easetype", "easeInQuart", "time", 0.3f, "oncomplete", "DisableMenu", "ignoretimescale", true));
	}

	private void DisableMenu()
	{
		ClearTab();

		gameObject.SetActive(false);
	}

	#endregion

	#region 메뉴 내부의 기능

	private MainMenuTab openedTab;
	private MainMenuButton openedButton;

	public void SwitchTab(MainMenuTab tab)
	{
		//  같은 탭으로 호출
		if (openedTab == tab && Is_Open)
		{
			CloseMainMenu();
		}

		//  다른 탭으로 교체
		else
		{
			//  열려있는거 끄고
			if (openedTab != null)
				openedTab.DisableButtons();

			//  열릴거 키고
			tab.EnableButtons();
			openedTab = tab;
		}
	}

	private void ClearTab()
	{
		if (openedTab != null)
			openedTab.DisableButtons();

		openedTab = null;
	}

	public void SwitchButton(MainMenuButton button)
	{
		if (openedButton != button)
		{
			//  열려있는거 끄고
			if (openedButton != null)
				openedButton.DisablePanel();

			//  열릴거 키고
			button.EnablePanel();
			openedButton = button;
		}
	}

	#endregion
}
