using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
	[SerializeField]
	private MainMenu mainMenu;
	[SerializeField]
	private GameObject panel;

	private void Start()
	{
		mainMenu = GetComponentInParent<MainMenu>();
	}

	public void OnClick()
	{
		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.Click);

		//  작용
		mainMenu.SwitchButton(this);
	}

	public void EnablePanel()
	{
		panel.SetActive(true);
	}

	public void DisablePanel()
	{
		panel.SetActive(false);
	}
}
