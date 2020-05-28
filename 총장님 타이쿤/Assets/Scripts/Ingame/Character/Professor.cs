using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum NAMES_PROFESSOR
{
	none = -1,
	엄숙한,
	근엄한,
	진지한,
	유쾌한,
	박식한,
	유명한,
	선량한
}

public class Professor : Person
{
	public Major BelongingMajor;

	public int Fame;
	public int Task;
	public int Teaching;
	public int Researching;
	public int PayPerMonth;

	private void Start()
	{
		uiCharacter = PeopleManager.Instance.GenerateCharacterForUI(this);
		uiCharacter.transform.localPosition = new Vector3(0.0f, 50.0f, -10.0f);

		Name = (NAMES_PROFESSOR.none + Random.Range(1, (int)NAMES_PROFESSOR.선량한)).ToString();
		NameText.text = Name;

		BelongingMajor = DeptManager.Instance.GetEstablishedMajor();
		if (BelongingMajor == null)  //  학과가 없으면 신설될 때 까지 기다림
		{
			DeptManager.Instance.AddNewDeptEvent(ResetDept);
			MajorText.text = "학과 미정";
		}
		else
			MajorText.text = BelongingMajor.name;

		Fame = Random.Range(20, 99);
		Task = Random.Range(20, 99);
		Teaching = Random.Range(20, 99);
		Researching = Random.Range(20, 99);
		PayPerMonth = (Fame + Task + Teaching + Researching) * 30 + 2000;
		UpdateText();
	}

	#region UI 작용

	public TextMeshProUGUI MajorText;
	public TextMeshProUGUI FameText;
	public TextMeshProUGUI TaskText;
	public TextMeshProUGUI TeachingText;
	public TextMeshProUGUI ResearchingText;
	public TextMeshProUGUI PayPerMonthText;

	public void ResetDept()
	{
		BelongingMajor = DeptManager.Instance.GetEstablishedMajor();

		if (BelongingMajor)
		{
			DeptManager.Instance.DeleteNewDeptEvent(ResetDept);
			MajorText.text = BelongingMajor.name;
		}
	}

	public void OnRecruit()
	{
		GetComponentInParent<RecruitProfessor>().OnRecruitClick(this);

		//  Professor를 피플 산하로
		transform.SetParent(PeopleManager.Instance.PeopleInGame.transform);
		BelongingMajor.AddProfessor(this);

		Destroy(GetComponentInChildren<Button>().gameObject);

		//  필드 캐릭터
		Character = PeopleManager.Instance.CopyCharacter(uiCharacter);
		Character.AddComponent<CharacterFSM>().State = CharacterState.Idle;
		SetInfoCard(Instantiate(InfoCardPrefab, Character.transform).GetComponent<CharacterInfoCard>());
	}

	public void UpdateText()
	{
		FameText.text = Fame.ToString();
		TaskText.text = Task.ToString();
		TeachingText.text = Teaching.ToString();
		ResearchingText.text = Researching.ToString();
		PayPerMonthText.text = "$" + string.Format("{0:#,###}", PayPerMonth) + " / 월";
	}

	#endregion

	#region 인포 카드

	public override void SetInfoCard(CharacterInfoCard infoCard)
	{
		infoCard.Original = this;

		infoCard.NameText.text = Name;
		infoCard.MajorText.text = BelongingMajor.name;
		infoCard.FameText.text = Fame.ToString();
		infoCard.TaskText.text = Task.ToString();
		infoCard.TeachingText.text = Teaching.ToString();
		infoCard.ResearchingText.text = Researching.ToString();
		infoCard.PayPerMonthText.text = "$" + string.Format("{0:#,###}", PayPerMonth) + " / 월";
	}

	#endregion

	#region 이벤트

	public override void MonthlyEvent()
	{
		PayEvent();
	}

	private void PayEvent()
	{
		GameManager.Instance.SpendMoney(PayPerMonth);
	}

	#endregion
}
