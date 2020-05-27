using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventName
{
	none = -1,
	월급날,
	개강,
	종강,
	학교평가
}

public class ScheduleManager : MonoBehaviour
{
	#region Singleton

	private static volatile ScheduleManager instance;
	private static object _lock = new System.Object();

	public static ScheduleManager Instance
	{
		get
		{
			if (instance != null)
				return instance;

			instance = FindObjectOfType<ScheduleManager>();

			if (instance != null)
				return instance;

			CreateThis();

			return instance;
		}
	}

	public static ScheduleManager CreateThis()
	{
		GameObject ScheduleManagerGameObject = new GameObject("Schedule");

		//  하나의 스레드로만 접근 가능하도록 lock
		lock (_lock)
			instance = ScheduleManagerGameObject.AddComponent<ScheduleManager>();

		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	#endregion

	#region Subject of Observers

	public delegate void TutorialEvent(bool flag);
	private event TutorialEvent onTutorialEvent;

	public void AddTutorialEvent(TutorialEvent func)
	{
		onTutorialEvent += func;
	}

	public void DeleteTutorialEvent(TutorialEvent func)
	{
		if (onTutorialEvent != null)
			onTutorialEvent -= func;
	}

	public delegate void MonthlyEvent();
	private event MonthlyEvent onMonthEvent;

	public void AddMonthEvent(MonthlyEvent func)
	{
		onMonthEvent += func;
	}

	public void DeleteMonthEvent(MonthlyEvent func)
	{
		if (onMonthEvent != null)
			onMonthEvent -= func;
	}

	#endregion

	public WorldTimer GameTime;

	public Dictionary<System.DateTime, Schedule> EventList = new Dictionary<System.DateTime, Schedule>();

	void Start()
	{
		Schedule eventItem;

		#region 정기 이벤트

		for (int y = 2020; y < 2024; y++)
		{
			for (int m = 1; m < 13; m++)
			{
				//  개강
				eventItem = new Schedule();
				eventItem.Name = EventName.월급날;
				eventItem.EventDate = new System.DateTime(y, m, 25);
				eventItem.EventContent = "교수님 월급 드리는날";
				eventItem.OnToday += Payday;
				EventList.Add(eventItem.EventDate, eventItem);
			}

			//  개강
			eventItem = new Schedule();
			eventItem.Name = EventName.개강;
			eventItem.EventDate = new System.DateTime(y, 3, 2);
			eventItem.EventContent = "개강하는 날!";
			eventItem.OnToday += StartSemester;
			EventList.Add(eventItem.EventDate, eventItem);

			//  종강
			eventItem = new Schedule();
			eventItem.Name = EventName.종강;
			eventItem.EventDate = new System.DateTime(y, 6, 14);
			eventItem.EventContent = "종강하는 날!";
			eventItem.OnToday += EndSemester;
			EventList.Add(eventItem.EventDate, eventItem);

			//  개강
			eventItem = new Schedule();
			eventItem.Name = EventName.개강;
			eventItem.EventDate = new System.DateTime(y, 9, 2);
			eventItem.EventContent = "개강하는 날!";
			eventItem.OnToday += StartSemester;
			EventList.Add(eventItem.EventDate, eventItem);

			//  종강
			eventItem = new Schedule();
			eventItem.Name = EventName.종강;
			eventItem.EventDate = new System.DateTime(y, 12, 14);
			eventItem.EventContent = "종강하는 날!";
			eventItem.OnToday += EndSemester;
			EventList.Add(eventItem.EventDate, eventItem);

			//  학교 평가
			eventItem = new Schedule();
			eventItem.Name = EventName.학교평가;
			eventItem.EventDate = new System.DateTime(y, 12, 31);
			eventItem.EventContent = "학교 평가";
			eventItem.OnToday += EvaluateSchool;
			EventList.Add(eventItem.EventDate, eventItem);

			#endregion

			#region 요구 이벤트

			eventItem = new Schedule();
			eventItem.EventDate = new System.DateTime(y, 4, 15);
			eventItem.EventContent = "";
			eventItem.OnToday += PeopleManager.Instance.AddOpinion;
			EventList.Add(eventItem.EventDate, eventItem);

			eventItem = new Schedule();
			eventItem.EventDate = new System.DateTime(y, 5, 24);
			eventItem.EventContent = "";
			eventItem.OnToday += PeopleManager.Instance.AddOpinion;
			EventList.Add(eventItem.EventDate, eventItem);

			eventItem = new Schedule();
			eventItem.EventDate = new System.DateTime(y, 7, 7);
			eventItem.EventContent = "";
			eventItem.OnToday += PeopleManager.Instance.AddOpinion;
			EventList.Add(eventItem.EventDate, eventItem);

			eventItem = new Schedule();
			eventItem.EventDate = new System.DateTime(y, 10, 11);
			eventItem.EventContent = "";
			eventItem.OnToday += PeopleManager.Instance.AddOpinion;
			EventList.Add(eventItem.EventDate, eventItem);

		}

		#endregion
	}

	#region 스케쥴 매니지먼트

	public void CheckSchedule(System.DateTime date)
	{
		if (EventList.ContainsKey(date))
		{
			EventList[date].CheckSchedule();
		}
	}

	public Schedule GetSchedule(System.DateTime date)
	{
		if (!EventList.ContainsKey(date))
			return null;

		return EventList[date];
	}

	#endregion

	#region 사전 생성된 이벤트

	public void EndTutorial()
	{
		GameTime.SetTimeSpeed(1.8f);
	}

	public void Payday()  //  건물 유지비 추가
	{
		DeptManager.Instance.Payday();
	}

	public void StartSemester()
	{
		GameManager.Instance.StartSemester();
		DeptManager.Instance.StartSemester();
	}

	public void EndSemester()
	{
		GameManager.Instance.EndSemester();
	}

	public void EvaluateSchool()
	{
		GameManager.Instance.EvaluateSchool();
	}

	#endregion
}

public class Schedule
{
	public EventName Name;
	public System.DateTime EventDate;
	public string EventContent;

	public delegate void EventDelegater();
	public event EventDelegater OnToday;

	public void CheckSchedule()
	{
		OnToday?.Invoke();
	}
}