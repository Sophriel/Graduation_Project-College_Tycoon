using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitProfessor : MonoBehaviour
{
	public Transform Layout;
	public GameObject ProfessorPrefab;
	public GameObject NoProfessor;

	private List<GameObject> ResumeList;

	private void Start()
	{
		ResumeList = new List<GameObject>();

		UpdateProfessors();
		DeptManager.Instance.AddNewDeptEvent(UpdateProfessors);
	}

	public void UpdateProfessors()
	{
		if (DeptManager.Instance.GetEstablishedMajor())
		{
			for (int i = 0; i < 3; i++)
				ResumeList.Add(Instantiate(ProfessorPrefab, Layout));

			NoProfessor.SetActive(false);
			DeptManager.Instance.DeleteNewDeptEvent(UpdateProfessors);
		}

		else
		{
			NoProfessor.SetActive(true);
		}
	}

	public void OnRerollClick()
	{
		//  돈 깎고

		UpdateProfessors();
	}

	public void OnRecruitClick()
	{

	}
}
