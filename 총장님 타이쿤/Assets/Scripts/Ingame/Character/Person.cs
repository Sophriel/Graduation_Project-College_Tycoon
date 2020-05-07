using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Person : MonoBehaviour
{
	protected bool IsPreGenerated;
	protected GameObject character;
	protected GameObject uiCharacter;

	public GameObject InfoCardPrefab;

	public string Name;
	public TextMeshProUGUI NameText;

	private void Start()
	{
		uiCharacter = PeopleManager.Instance.GenerateCharacterForUI(this);
	}

	public virtual void SetInfoCard(CharacterInfoCard infoCard)
	{

	}
}
