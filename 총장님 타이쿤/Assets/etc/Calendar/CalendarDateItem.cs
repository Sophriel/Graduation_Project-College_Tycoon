using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CalendarDateItem : MonoBehaviour
{
	private TextMeshProUGUI date;

	public EventName Name;
	public string EventContent;

	private void Start()
	{
		date = gameObject.GetComponentInChildren<TextMeshProUGUI>();
	}

	public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(date.text, EventContent);
    }
}

// 이벤트 종류
// 개강
// 종강
//  학교 평가