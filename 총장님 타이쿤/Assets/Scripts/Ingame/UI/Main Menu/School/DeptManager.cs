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
		GameObject DeptManagerGameObject = new GameObject("Department");

		//  하나의 스레드로만 접근 가능하도록 lock
		lock (_lock)
			instance = DeptManagerGameObject.AddComponent<DeptManager>();

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

	//  세미나가 개최될 때 마다 호출
	public delegate void SeminarEvent();
	private event SeminarEvent onSeminarEvent;

	public void AddSeminarEvent(SeminarEvent func)
	{
		onSeminarEvent += func;
	}

	public void DeleteSeminarEvent(SeminarEvent func)
	{
		if (onSeminarEvent != null)
			onSeminarEvent -= func;
	}

	#endregion

	private void OnEnable()
	{
		EstMenu.SetActive(false);
		UnEstMenu.SetActive(false);
	}

	#region UI 멤버

	public bool IsEstablishMode = false;

	public Transform CollegeGrid { get; private set; }
	public Transform DeptGrid { get; private set; }

	public GameObject EstMenu;
	public GameObject UnEstMenu;
	public GameObject AssignButton;
	public GameObject ShowBuildingButton;

	public College CurrentCollege { get; private set; }
	public Department CurrentDept;

	#endregion

	#region UI 표시

	public void ChangeMode()
	{
		//  설립모드로 변경
		if (!IsEstablishMode)
		{
			IsEstablishMode = true;
		}

		//  기존으로 변경
		else
			IsEstablishMode = false;

		onPlusButtonEvent?.Invoke();
		onCollegeButtonEvent?.Invoke();

		EstMenu.SetActive(false);
		UnEstMenu.SetActive(false);
	}

	//  학부 선택할때 오른쪽 전공 패널 수정
	public void SwitchMajorPannel(College nextCollege)
	{
		CurrentCollege = nextCollege;
		onCollegeButtonEvent?.Invoke();

		CurrentCollege.SetActiveThis();

		EstMenu.SetActive(false);
		UnEstMenu.SetActive(false);
	}

	public void ShowMenu(bool IsDeptEstablished)
	{
		if (IsDeptEstablished)
		{
			UnEstMenu.SetActive(false);
			EstMenu.SetActive(true);

			//if (!CurrentDept.AssignedBuilding)
			//{
			//	//ShowBuildingButton.SetActive(false);
			//	AssignButton.SetActive(true);
			//}

			//else
			//{
			//	AssignButton.SetActive(false);
			//	//ShowBuildingButton.SetActive(true);
			//}
		}

		else
		{
			EstMenu.SetActive(false);
			UnEstMenu.SetActive(true);
		}
	}

	#endregion

	#region 메뉴 

	public void CurrentDept_ShowInfo()
	{

	}

	public void CurrentDept_Establish()
	{
		if (CurrentDept.Establish())
		{
			onTutorialEvent?.Invoke(true);
			onNewDeptEvent?.Invoke();
			ShowMenu(CurrentDept.IsEstablished);
		}
	}

	public void CurrentDept_AssignBuilding()
	{
		//  메뉴를 꺼
		GameManager.Instance.CloseUI();

		//  마우스매니저한테 어사인 연락 받아야해
		MouseManager.Instance.AddAssignEvent(AssignConfirm);

		//  마우스는 어사인모드
		MouseManager.Instance.StartAssigning();
	}

	public void AssignConfirm(GameObject building)
	{
		if (!building)
		{
			MouseManager.Instance.DeleteAssignEvent(AssignConfirm);
			return;
		}

		Building temp = building.GetComponent<Building>();
		if (!temp)
		{
			MouseManager.Instance.DeleteAssignEvent(AssignConfirm);
			return;
		}

		if (!temp.Owner)
		{
			temp.Owner = CurrentDept;
			CurrentDept.AssignedBuilding.Add(building);
			MouseManager.Instance.DeleteAssignEvent(AssignConfirm);

		}
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

	public void CurrentDept_Seminar()
	{
		if (!GameManager.Instance.CanSpendMoney(50000))
		{
			return;
		}

		CurrentDept.Seminar();
		onSeminarEvent?.Invoke();
		onSeminarEvent = null;
	}

	public void CurrentDept_Close()
	{
		CurrentDept.Close();
	}

	#endregion

	#region 매니지먼트 멤버

	public ResearchList Researcher;

	public List<College> EstablishedCollege = new List<College>();
	public List<Major> EstablishedMajor = new List<Major>();

	#endregion

	#region 부서 매니지먼트

	public College GetEstablishedCollege()
	{
		if (EstablishedCollege.Count > 0)
			return EstablishedCollege[Random.Range(0, EstablishedCollege.Count)];

		return null;
	}

	public Major GetEstablishedMajor()
	{
		if (EstablishedMajor.Count > 0)
			return EstablishedMajor[Random.Range(0, EstablishedMajor.Count)];

		return null;
	}

	public void Payday()
	{
		foreach (Major m in EstablishedMajor)
		{
			m.Payday();
		}
	}

	public void StartSemester()
	{
		foreach (Major m in EstablishedMajor)
		{
			m.StartSemester();
		}
	}

	public void EvaluateSchool()
	{
		GameManager.Instance.FamePoint = 0;
		GameManager.Instance.TeachPoint = 0;
		
		foreach (Major m in EstablishedMajor)
		{
			GameManager.Instance.FamePoint += m.WholeFame;
			foreach (Professor p in m.Professors)
				GameManager.Instance.TeachPoint += p.Teaching;
			GameManager.Instance.SatisfactionPoint += (m.AssignedBuilding.Count * 30) - m.StudentsCapacity;
		}

		GameManager.Instance.FamePoint /= EstablishedMajor.Count;
	}

	#endregion
}
