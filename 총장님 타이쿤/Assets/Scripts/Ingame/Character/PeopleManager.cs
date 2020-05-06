using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public List<Person> PeopleInGame = new List<Person>();

	#region 캐릭터

	public GameObject MalePrefab;
	public GameObject FemalePrefab;

	public GameObject GenerateCharacter(Person person)
	{
		GameObject character = null;

		switch (Random.Range(0, 1))
		{
			case 0:
				character = Instantiate(MalePrefab, person.transform);
				break;

			case 1:
				character = Instantiate(FemalePrefab, person.transform);
				break;

			default:
				break;
		}

		return character;
	}

	public void SetCharacterForUI(GameObject gameObject)
	{
		gameObject.transform.localScale *= 100.0f;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.up * 200.0f);
		gameObject.layer = LayerMask.NameToLayer("UI");

		foreach (Transform item in gameObject.GetComponentsInChildren<Transform>())
			item.gameObject.layer = LayerMask.NameToLayer("UI");

	}

	#endregion

}
