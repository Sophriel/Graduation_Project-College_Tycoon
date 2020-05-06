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

	private void Start()
	{
		korName = GetComponentInChildren<TextMeshProUGUI>();

		if (!IsEstablished)
			korName.color = FaintColor;

		gameObject.SetActive(false);
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

}
