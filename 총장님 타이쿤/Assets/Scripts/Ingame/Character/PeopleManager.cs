using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	#endregion

	#region Person 매니지먼트

	public int StudentsCount;
	public int ProfessorsCount;
	public GameObject PeopleInGame;

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
