using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Opinion : MonoBehaviour
{
	public Image CurrentBG;
	public TextMeshProUGUI OpinionContent;

	public void Solve()
	{
		PeopleManager.Instance.StudentsSatisfaction += 200;
		PeopleManager.Instance.ProfessorsSatisfaction += 200;

		MouseManager.Instance.DeleteBuildEvent(Solve);
		Destroy(gameObject);
	}

	void Start()
    {
		CurrentBG = GetComponent<Image>();
		OpinionContent = GetComponentInChildren<TextMeshProUGUI>();
    }

}