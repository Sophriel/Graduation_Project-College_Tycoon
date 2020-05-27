using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum NAMES_COLLEGES
{
	none = -1,
	게임학부,
	조형대학,
	과학기술대학,
	광고홍보학부
}

public class College : Department
{
	public Major[] SubDepts;


	private void Start()
	{
		DeptManager.Instance.AddPlusButtonEvent(SetActiveThis);

		KorName = GetComponentInChildren<TextMeshProUGUI>();

		foreach (Major i in SubDepts)
			i.UpperCollege = this;

		if (!DeptManager.Instance.IsEstablishMode && !IsEstablished)
		{
			KorName.color = FaintColor;
			gameObject.SetActive(false);
		}
	}

	public override void OnClick()
	{
		DeptManager.Instance.SwitchMajorPannel(this);

		foreach (Major i in SubDepts)
			i.SetActiveThis();

		DeptManager.Instance.ShowMenu(IsEstablished);
		DeptManager.Instance.CurrentDept = this;
	}

	public override void SetActiveThis()
	{
		if (!DeptManager.Instance.IsEstablishMode && !IsEstablished)
			gameObject.SetActive(false);

		else
			gameObject.SetActive(true);
	}

	public override bool Establish()
	{
		if (IsEstablished)
			return false;

		KorName.color = DeepColor;
		IsEstablished = true;

		DeptManager.Instance.EstablishedCollege.Add(this);

		return true;

		//  돈이 부족하면 return false;
	}
}
