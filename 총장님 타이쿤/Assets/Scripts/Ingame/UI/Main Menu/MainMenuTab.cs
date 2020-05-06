using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTab : MonoBehaviour
{
	private MainMenu mainMenu;
	[SerializeField]
	private GameObject buttonGroup;
	[SerializeField]
	private MainMenuButton outlineButton;

	private void Start()
	{
		mainMenu = GetComponentInParent<MainMenu>();

		buttonGroup.SetActive(false);
	}

	public void OnClick()
	{
		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Click);

		//  메뉴 닫혀있으면 여는것도
		if (!mainMenu.Is_Open)
			mainMenu.OpenMainMenu();

		//  작용
		mainMenu.SwitchTab(this);
		mainMenu.SwitchButton(outlineButton);
	}

	public void EnableButtons()
	{
		buttonGroup.SetActive(true);
	}

	public void DisableButtons()
	{
		buttonGroup.SetActive(false);
	}
}
