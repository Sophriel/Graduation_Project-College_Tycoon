using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Person : MonoBehaviour
{
	public GameObject Character;
	protected GameObject uiCharacter;

	public GameObject InfoCardPrefab;

	public string Name;
	public TextMeshProUGUI NameText;

	public virtual void SetInfoCard(CharacterInfoCard infoCard) { }
	public virtual void MonthlyEvent() { }
}
