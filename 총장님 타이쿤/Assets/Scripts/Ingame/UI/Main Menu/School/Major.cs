using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum NAMES_MAJORS
{
	none = -1,
	게임소프트웨어,
	게임그래픽디자인,
	디자인컨버전스,
	영상애니메이션
}

public class Major : Department
{
	public NAMES_MAJORS MajorName;
	public College UpperCollege;

	public List<Student> Students;
	public List<Professor> Professors;

	public int WholeFame = 0;
	public int StudentsCapacity = 0;
	public int ClassQuality = 0;
	public int ResearchSpeed = 0;

	private void Start()
	{
		KorName = GetComponentInChildren<TextMeshProUGUI>();

		if (!IsEstablished)
			KorName.color = FaintColor;

		gameObject.SetActive(false);

		Students = new List<Student>();
		Professors = new List<Professor>();
	}

	#region UI

	public override void OnClick()
	{
		DeptManager.Instance.ShowMenu(IsEstablished);
		DeptManager.Instance.CurrentDept = this;
	}

	public override void SetActiveThis()
	{
		//  설립모드x, 설립상태x, 현재 선택됨x
		if (!DeptManager.Instance.IsEstablishMode && !IsEstablished || DeptManager.Instance.CurrentCollege != UpperCollege)
		{
			DeptManager.Instance.DeleteCollegeButtonEvent(SetActiveThis);
			gameObject.SetActive(false);
		}

		else
		{
			gameObject.SetActive(true);
			DeptManager.Instance.AddCollegeButtonEvent(SetActiveThis);
		}
	}

	#endregion

	#region 작용

	public override bool Establish()
	{
		if (IsEstablished)
			return false;

		//  상위 대학이 설립된 경우에만
		if (UpperCollege.IsEstablished)
		{
			switch (KorName.text)
			{
				case "게임소프트웨어":
				case "게임그래픽디자인":
					if (!GameManager.Instance.CanSpendMoney(30000))
						return false;
					GameManager.Instance.SpendMoney(30000);
					break;
				case "디자인 컨버전스":
				case "영상 애니메이션":
					if (!GameManager.Instance.CanSpendMoney(100000))
						return false;
					GameManager.Instance.SpendMoney(100000);
					break;
				default:
					return false;
			}

			KorName.color = DeepColor;
			IsEstablished = true;

			DeptManager.Instance.EstablishedMajor.Add(this);
			return true;
		}

		return false;
	}

	public void AddProfessor(Professor professor)
	{
		Professors.Add(professor);

		WholeFame = 0;
		foreach (Professor p in Professors)
		{
			WholeFame += professor.Fame;
		}
		WholeFame /= Professors.Count;
		StudentsCapacity += (professor.Task + professor.Teaching) / 10;
		ClassQuality += professor.Teaching * (professor.Fame / 20);
		ResearchSpeed += professor.Researching * (professor.Fame / 20);
	}

	public override bool Seminar(Seminar seminar)
	{
		if (!GameManager.Instance.CanSpendMoney(10000 * Professors.Count))
		{
			return false;
		}

		GameManager.Instance.SpendMoney(10000 * Professors.Count);
		seminar.StartSemninar(Professors);

		return true;
	}

	#endregion

	#region 정기 이벤트

	public void Payday()
	{
		foreach (Professor p in Professors)
		{
			GameManager.Instance.SpendMoney(p.PayPerMonth);
		}
	}

	public void StartSemester()
	{
		while (StudentsCapacity > Students.Count)
		{
			Students.Add(PeopleManager.Instance.GenerateStudent(this));
		}

		foreach (Student p in Students)
		{
			GameManager.Instance.EarnMoney(4500);
		}

	}

	#endregion
}
