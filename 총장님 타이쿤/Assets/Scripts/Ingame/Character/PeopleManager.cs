using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PeopleManager : MonoBehaviour
{
	#region Singleton

	private static volatile PeopleManager instance;
	private static object _lock = new System.Object();

	public static PeopleManager Instance
	{
		get
		{
			if (instance != null)
				return instance;

			instance = FindObjectOfType<PeopleManager>();

			if (instance != null)
				return instance;

			CreateThis();

			return instance;
		}
	}

	public static PeopleManager CreateThis()
	{
		GameObject MouseManagerGameObject = new GameObject("PeopleManager");

		//  하나의 스레드로만 접근 가능하도록 lock
		lock (_lock)
			instance = MouseManagerGameObject.AddComponent<PeopleManager>();

		return instance;
	}

	void Awake()
	{
		instance = this;
	}

	#endregion

	#region UI

	public TextMeshProUGUI StudentsText;
	public TextMeshProUGUI ProfessorText;
	public TextMeshProUGUI PeopleText;

	private void UpdatePeopleText()
	{
		StudentsText.text = StudentsCount.ToString();
		ProfessorText.text = ProfessorsCount.ToString();
		PeopleText.text = (StudentsCount + ProfessorsCount).ToString();
	}

	public Sprite GoodBG;
	public Sprite BadBG;
	public Sprite GoodFace;
	public Sprite BadFace;

	public Image StudentBG;
	public Image StudentFace;
	public Image StudentBar;
	public Image ProfessorBG;
	public Image ProfessorFace;
	public Image ProfessorBar;

	private void Update()
	{
		UpdateSatisfaction();
	}

	private void UpdateSatisfaction()
	{
		if (StudentsSatisfaction > 1000)
			StudentsSatisfaction = 1000;
		else if (StudentsSatisfaction < 0)
			StudentsSatisfaction = 0;

		if (StudentsSatisfaction > 500)
		{
			StudentBG.sprite = GoodBG;
			StudentFace.sprite = GoodFace;
		}
		else if (StudentsSatisfaction > 0)
		{
			StudentBG.sprite = BadBG;
			StudentFace.sprite = BadFace;
		}

		StudentBar.rectTransform.offsetMax = new Vector2(-1110f + (430 * StudentsSatisfaction / 1000f), 208f);
		StudentBar.color = new Color(1.0f - (StudentsSatisfaction / 1000f), StudentsSatisfaction / 1000f, 0.0f);


		if (ProfessorsSatisfaction > 1000)
			ProfessorsSatisfaction = 1000;
		else if (ProfessorsSatisfaction < 0)
			ProfessorsSatisfaction = 0;

		if (ProfessorsSatisfaction > 500)
		{
			ProfessorBG.sprite = GoodBG;
			ProfessorFace.sprite = GoodFace;
		}
		else if (ProfessorsSatisfaction > 0)
		{
			ProfessorBG.sprite = BadBG;
			ProfessorFace.sprite = BadFace;
		}

		ProfessorBar.rectTransform.offsetMax = new Vector2(-490f + (430 * ProfessorsSatisfaction / 1000f), 208);
		ProfessorBar.color = new Color(1 - (ProfessorsSatisfaction / 1000f), ProfessorsSatisfaction / 1000f, 0.0f);
	}

	public void SolveProblem()
	{

	}

	public GameObject OpinionPrefab;
	private int opNum = 0;
	public Transform StudentOpinionLayout;
	public Transform ProfessorOpinionLayout;

	public void AddOpinion()
	{
		switch (opNum)
		{
			case 0:
				NeedResearch();
				break;
			case 1:
				NeedMoreBuilding();
				break;
			case 2:
				NeedSeminar();
				break;
			case 3:
				NeedMoreProfessor();
				break;
			default:
				break;
		}

		opNum++;
	}

	#endregion

	#region 오피니언 이벤트 목록

	private void NeedResearch()
	{
		Opinion opinion = Instantiate(OpinionPrefab, ProfessorOpinionLayout).GetComponent<Opinion>();

		opinion.CurrentBG.sprite = BadBG;
		opinion.OpinionContent.text = "연구를 진행해주세요!";
		DeptManager.Instance.Researcher.OnResearchStart += opinion.Solve;
	}

	private void NeedMoreBuilding()
	{
		Opinion opinion = Instantiate(OpinionPrefab, StudentOpinionLayout).GetComponent<Opinion>();

		opinion.CurrentBG.sprite = BadBG;
		opinion.OpinionContent.text = "학교 건물이 너무 적어요!";
		MouseManager.Instance.AddBuildEvent(opinion.Solve);
	}

	private void NeedSeminar()
	{
		Opinion opinion = Instantiate(OpinionPrefab, ProfessorOpinionLayout).GetComponent<Opinion>();

		opinion.CurrentBG.sprite = BadBG;
		opinion.OpinionContent.text = DeptManager.Instance.EstablishedMajor[0].Professors[0].Name + " 교수는 세미나를 원합니다!";
		DeptManager.Instance.AddSeminarEvent(opinion.Solve);
	}

	private void NeedMoreProfessor()
	{
		Opinion opinion = Instantiate(OpinionPrefab, StudentOpinionLayout).GetComponent<Opinion>();

		opinion.CurrentBG.sprite = BadBG;
		opinion.OpinionContent.text = "교수님이 더 있으면 좋겠습니다";
		Recruiter.OnRecruitEvent += opinion.Solve;
	}

	#endregion

	#region Person 매니지먼트

	public RecruitProfessor Recruiter;

	public int StudentsCount;
	public int ProfessorsCount;
	public GameObject PeopleInGame;

	public int StudentsSatisfaction;
	public int ProfessorsSatisfaction;

	public void EvaluateSchool()
	{
		GameManager.Instance.SatisfactionPoint = StudentsSatisfaction + ProfessorsSatisfaction;
	}

	#endregion

	#region 캐릭터

	public GameObject MalePrefab;
	public GameObject FemalePrefab;

	public GameObject StudentInfoPrefab;

	public GameObject GenerateCharacter()
	{
		GameObject character = null;

		switch (Random.Range(0, 2))
		{
			case 0:
				character = Instantiate(MalePrefab);
				break;

			case 1:
				character = Instantiate(FemalePrefab);
				break;

			default:
				break;
		}

		return character;
	}

	public GameObject CopyCharacter(GameObject original)
	{
		GameObject character = Instantiate(original);
		Destroy(character.GetComponent<CharacterCustomization>());

		character.transform.localScale = Vector3.one;
		character.transform.position = new Vector3(-42.0f, -0.05f, -39.0f);

		character.layer = LayerMask.NameToLayer("Person");

		foreach (Transform item in character.GetComponentsInChildren<Transform>())
			item.gameObject.layer = LayerMask.NameToLayer("Person");

		ProfessorsCount++;
		UpdatePeopleText();

		return character;
	}

	public GameObject GenerateCharacterForUI(Person person)
	{
		GameObject character = GenerateCharacter();
		Destroy(character.GetComponent<CharacterFSM>());

		character.transform.SetParent(person.transform);

		character.transform.localScale *= 0.8f;
		character.transform.localRotation = Quaternion.Euler(Vector3.up * 200.0f);
		character.layer = LayerMask.NameToLayer("UI");

		foreach (Transform item in gameObject.GetComponentsInChildren<Transform>())
			item.gameObject.layer = LayerMask.NameToLayer("UI");

		return character;
	}

	public Student GenerateStudent(Major major)
	{
		GameObject character = GenerateCharacter();
		Student comp = character.AddComponent<Student>();
		comp.Character = character;
		comp.BelongingMajor = major;
		comp.InfoCardPrefab = StudentInfoPrefab;

		character.transform.localScale = Vector3.one;
		character.transform.position = new Vector3(-42.0f, -0.05f, -39.0f);

		character.layer = LayerMask.NameToLayer("Person");

		foreach (Transform item in character.GetComponentsInChildren<Transform>())
			item.gameObject.layer = LayerMask.NameToLayer("Person");

		StudentsCount++;
		UpdatePeopleText();

		return comp;
	}

	#endregion
}
