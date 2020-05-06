using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Person : MonoBehaviour
{
	protected bool IsPreGenerated;
	protected GameObject character;
	public CharacterFSM FSM;

	public string Name;
	public TextMeshProUGUI NameText;

	private void Start()
	{
		character = PeopleManager.Instance.GenerateCharacter(this);
		FSM = GetComponent<CharacterFSM>();
	}
}
