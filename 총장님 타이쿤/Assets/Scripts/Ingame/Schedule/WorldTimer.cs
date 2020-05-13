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

	public System.DateTime DateTime { get; private set; }
	private float timeSpeed = 1.0f;
	private int timeScale = 1;
	public TextMeshProUGUI TimeSpeedUI;

    void Start()
    {
		PlayButton.GetComponent<Image>().color = staticBlueColor;

		DateTime = new System.DateTime(2020, 1, 1);
		timeSpeed = 0.2f;

		timeUI = GetComponentInChildren<TextMeshProUGUI>();
    }

	private void FixedUpdate()
	{
		System.DateTime day = DateTime;

		DateTime = DateTime.AddDays(0.02 * timeSpeed);
		timeUI.text = DateTime.ToString("yyyy.MM.dd");

		if (DateTime.Year > day.Year)
			ScheduleManager.Instance.CheckSchedule(DateTime.Date);
		else if (DateTime.Month > day.Month)
			ScheduleManager.Instance.CheckSchedule(DateTime.Date);		
		else if (DateTime.Day > day.Day)
			ScheduleManager.Instance.CheckSchedule(DateTime.Date);
	}

	public void SetTimeSpeed(float speed)
	{
		timeSpeed = speed;
	}

	public void OnPlayButton()
	{
		timeScale = 1;
		Time.timeScale = timeScale;

		TimeSpeedUI.text = "x" + timeScale.ToString();
	}

	public void OnPauseButton()
	{
		timeScale = 0;
		Time.timeScale = timeScale;

		TimeSpeedUI.text = "x" + timeScale.ToString();
	}

	public void OnFastForwardButton()
	{
		timeScale *= 2;
		Time.timeScale = timeScale;

		TimeSpeedUI.text = "x" + timeScale.ToString();
	}
}
