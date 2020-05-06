using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CalendarDateItem : MonoBehaviour {

	private TextMeshProUGUI tmp;

	private void Start()
	{
		tmp = gameObject.GetComponentInChildren<TextMeshProUGUI>();
	}

	public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(tmp.text);
    }
}
