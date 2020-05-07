using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitProfessor : MonoBehaviour
{
	public Transform Layout;
	public GameObject ProfessorPrefab;
	public GameObject RecruitButtonPrefab;
	public GameObject InfoCardPrefab;
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
			NoProfessor.SetActive(false);
			Layout.gameObject.SetActive(true);

			for (int i = 0; i < 3; i++)
			{
				ResumeList.Add(Instantiate(ProfessorPrefab, Layout));
				Professor temp = ResumeList[i].GetComponent<Professor>();
				Instantiate(RecruitButtonPrefab, ResumeList[i].transform).GetComponent<Button>().onClick.AddListener(temp.OnRecruit);
			}

			DeptManager.Instance.DeleteNewDeptEvent(UpdateProfessors);
		}

		else
		{
			Layout.gameObject.SetActive(false);
			NoProfessor.SetActive(true);
		}
	}

	public void OnRerollClick()
	{
		//  돈 깎고

		while (ResumeList.Count > 0)
		{
			Destroy(ResumeList[0]);
			ResumeList.RemoveAt(0);
		}

		UpdateProfessors();
	}

	public void OnRecruitClick(Professor professor)
	{
		ResumeList.Remove(professor.gameObject);

		if (ResumeList.Count == 0)
		{
			Layout.gameObject.SetActive(false);
			NoProfessor.SetActive(true);
		}
	}
}
