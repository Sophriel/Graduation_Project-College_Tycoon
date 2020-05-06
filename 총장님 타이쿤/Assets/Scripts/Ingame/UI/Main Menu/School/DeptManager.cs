using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeptManager : MonoBehaviour
{
	#region Singleton

	private static volatile DeptManager instance;
	private static object _lock = new System.Object();

	public static DeptManager Instance
	{
		get
		{
			if (instance != null)
				return instance;

			instance = FindObjectOfType<DeptManager>();

			if (instance != null)
				return instance;

			CreateThis();

			return instance;
		}
	}

	public static DeptManager CreateThis()
	{
		GameObject MouseManagerGameObject = new GameObject("Department");

		//  하나의 스레드로만 접근 가능하도록 lock
		lock (_lock)
			instance = MouseManagerGameObject.AddComponent<DeptManager>();

		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	#endregion

	#region Subject of Observers

	//  +버튼 눌릴때마다 호출
	public delegate void PlusButtonEvent();
	private event PlusButtonEvent onPlusButtonEvent;

	public void AddPlusButtonEvent(PlusButtonEvent func)
	{
		onPlusButtonEvent += func;
	}

	public void DeletePlusButtonEvent(PlusButtonEvent func)
	{
		if (onPlusButtonEvent != null)
			onPlusButtonEvent -= func;
	}

	//  학부 버튼이 눌릴때 마다 호출
	public delegate void CollegeButtonEvent();
	private event CollegeButtonEvent onCollegeButtonEvent;

	public void AddCollegeButtonEvent(CollegeButtonEvent func)
	{
		onCollegeButtonEvent += func;
	}

	public void DeleteCollegeButtonEvent(CollegeButtonEvent func)
	{
		if (onCollegeButtonEvent != null)
			onCollegeButtonEvent -= func;
	}

	//  Department가 신설될 때 마다 호출
	//  주로 Person이 이 이벤트에 관심있음
	public delegate void NewDeptEvent();
	private event NewDeptEvent onNewDeptEvent;

	public void AddNewDeptEvent(NewDeptEvent func)
	{
		onNewDeptEvent += func;
	}

	public void DeleteNewDeptEvent(NewDeptEvent func)
	{
		if (onNewDeptEvent != null)
			onNewDeptEvent -= func;
	}

	#endregion

	#region UI 멤버

	public bool IsEstablishMode = false;

	public Transform CollegeGrid { get; private set; }
	public Transform DeptGrid { get; private set; }

	[SerializeField]
	private GameObject estMenu;
	[SerializeField]
	private GameObject unEstMenu;

	public College CurrentCollege { get; private set; }
	public Department CurrentDept;

	#endregion


	private void Start()
	{
		estMenu.SetActive(false);
		unEstMenu.SetActive(false);
	}

	#region 매니지먼트 멤버

	public List<College> EstablishedCollege = new List<College>();
	public List<Major> EstablishedMajor = new List<Major>();

	#endregion

	#region UI 표시

	public void ChangeMode()
	{
		//  설립모드로 변경
		if (!IsEstablishMode)
			IsEstablishMode = true;

		//  기존으로 변경
		else
			IsEstablishMode = false;

		onPlusButtonEvent?.Invoke();
		onCollegeButtonEvent?.Invoke();

		estMenu.SetActive(false);
		unEstMenu.SetActive(false);
	}

	//  학부 선택할때 오른쪽 전공 패널 수정
	public void SwitchMajorPannel(College nextCollege)
	{
		CurrentCollege = nextCollege;
		onCollegeButtonEvent?.Invoke();

		CurrentCollege.SetActiveThis();

		estMenu.SetActive(false);
		unEstMenu.SetActive(false);
	}

	public void ShowMenu(bool IsDeptEstablished)
	{
		if (IsDeptEstablished)
			estMenu.SetActive(true);

		else
			unEstMenu.SetActive(true);
	}

	#endregion

	#region 메뉴 

	public void CurrentDept_ShowInfo()
	{
		
	}

	public void CurrentDept_Establish()
	{
		if (CurrentDept.Establish())
			onNewDeptEvent?.Invoke();
	}

	public void CurrentDept_ShowBuilding()
	{
		
	}

	public void CurrentDept_People()
	{

	}

	public void CurrentDept_Professors()
	{

	}

	public void CurrentDept_Researches()
	{

	}

	public void CurrentDept_Close()
	{
		CurrentDept.Close();
	}

	#endregion

	#region 부서 매니지먼트

	public Major GetEstablishedMajor()
	{
		if (EstablishedMajor.Count > 0)
			return EstablishedMajor[Random.Range(0, EstablishedMajor.Count - 1)];

		return null;
	}

	#endregion
}
