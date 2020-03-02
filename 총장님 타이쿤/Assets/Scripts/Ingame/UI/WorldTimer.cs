using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldTimer : MonoBehaviour
{
	public GameObject PlayButton;
	public GameObject PauseButton;
	public GameObject FastForwardButton;
	private static Color staticBlueColor = new Color(80.0f / 255, 123.0f / 255, 200.0f / 255);

	private TextMeshProUGUI timeUI;

	private System.DateTime dateTime;
	private int timeSpeed = 1;
	public TextMeshProUGUI TimeSpeedUI;

    void Start()
    {
		PlayButton.GetComponent<Image>().color = staticBlueColor;

		dateTime = new System.DateTime(2020, 1, 1);

		timeUI = GetComponentInChildren<TextMeshProUGUI>();
    }

	private void FixedUpdate()
	{
		dateTime = dateTime.AddSeconds(1.0);
		timeUI.text = dateTime.ToString("yyyy.MM.dd tt hh:mm");

	}

	void Update()
    {
	}

	public void OnPlayButton()
	{
		timeSpeed = 1;
		Time.timeScale = timeSpeed;

		TimeSpeedUI.text = "x" + timeSpeed.ToString();
	}

	public void OnPauseButton()
	{
		timeSpeed = 0;
		Time.timeScale = timeSpeed;

		TimeSpeedUI.text = "x" + timeSpeed.ToString();
	}

	public void OnFastForwardButton()
	{
		timeSpeed *= 2;
		Time.timeScale = timeSpeed;

		TimeSpeedUI.text = "x" + timeSpeed.ToString();
	}
}
