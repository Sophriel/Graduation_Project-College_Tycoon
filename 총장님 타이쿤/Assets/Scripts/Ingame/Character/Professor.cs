using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	public int Charisma;
	public int Teaching;
	public int Researching;
	public int PayPerMonth;

	#region UI 작용

	public TextMeshProUGUI MajorText;
	public TextMeshProUGUI FameText;
	public TextMeshProUGUI CharismaText;
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
		if (character != null)
		{
			character.transform.position = new Vector3(-42.0f, -0.05f, -39.0f);

		}

	}

	#endregion

	private void Start()
	{
		if (!IsPreGenerated)
		{
			character = PeopleManager.Instance.GenerateCharacter(this);
			PeopleManager.Instance.SetCharacterForUI(character);
			character.transform.localPosition = new Vector3(0.0f, 50.0f, -1.0f);

			FSM = character.GetComponent<CharacterFSM>();
			FSM.State = CharacterState.None;

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
			FameText.text = Fame.ToString();
			Charisma = Random.Range(20, 99);
			CharismaText.text = Charisma.ToString();
			Teaching = Random.Range(20, 99);
			TeachingText.text = Teaching.ToString();
			Researching = Random.Range(20, 99);
			ResearchingText.text = Researching.ToString();

			PayPerMonth = (Fame + Charisma + Teaching + Researching) * 3 + 200;
			PayPerMonthText.text = "$" + string.Format("{0:#,###}", PayPerMonth) + " / 월";
		}

		

	}

}
