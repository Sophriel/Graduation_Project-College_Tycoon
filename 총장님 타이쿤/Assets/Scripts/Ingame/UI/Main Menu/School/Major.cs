using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum NAMES_MAJORS
{
	none = -1,
	게임소프트웨어,
	게임그래픽디자인,
	디자인컨버전스,
	영상애니메이션
}

public class Major : Department
{
	public College UpperCollege;

	public List<Student> Students;
	public List<Professor> Professors;

	public int WholeFame = 0;
	public int StudentsCapacity = 0;
	public int ClassQuality = 0;
	public int ResearchSpeed = 0;

	private void Start()
	{
		korName = GetComponentInChildren<TextMeshProUGUI>();

		if (!IsEstablished)
			korName.color = FaintColor;

		gameObject.SetActive(false);

		Students = new List<Student>();
		Professors = new List<Professor>();
	}

	//  College의 OnClick을 오버로드
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

	public override bool Establish()
	{
		if (IsEstablished)
			return false;

		//  상위 대학이 설립된 경우에만
		if (UpperCollege.IsEstablished)
		{
			korName.color = DeepColor;
			IsEstablished = true;

			DeptManager.Instance.EstablishedMajor.Add(this);
			return true;
		}

		return false;
		//  돈이 부족하면 return false;
	}

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

	public void AddStudent()
	{

	}

	public void AddProfessor(Professor professor)
	{
		Professors.Add(professor);

		WholeFame += professor.Fame;
		StudentsCapacity += (professor.Task + professor.Teaching) / 4;
		ClassQuality += professor.Teaching * (professor.Fame / 20);
		ResearchSpeed += professor.Researching * (professor.Fame / 20);
	}
}
