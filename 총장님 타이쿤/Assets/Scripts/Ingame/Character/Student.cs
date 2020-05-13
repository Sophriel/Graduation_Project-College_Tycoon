using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : Person
{
	public Major BelongingMajor;

	void Start()
    {
		//  필드 캐릭터
		SetInfoCard(Instantiate(InfoCardPrefab, Character.transform).GetComponent<CharacterInfoCard>());
	}

	public override void SetInfoCard(CharacterInfoCard infoCard)
	{
		infoCard.Original = this;

		infoCard.NameText.text = Name;
		infoCard.MajorText.text = BelongingMajor.name;
	}
}
