using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfoCard : MonoBehaviour
{
	public Person Original;

	public TextMeshPro NameText;
	public TextMeshPro MajorText;
	public TextMeshPro PayPerMonthText;
	public TextMeshPro FameText;
	public TextMeshPro TaskText;
	public TextMeshPro TeachingText;
	public TextMeshPro ResearchingText;

	private void Start()
	{
		transform.localScale = Vector3.zero;
		transform.localPosition += Vector3.up * 10.0f;
	}

	private void Update()
	{
		transform.LookAt(GameManager.Instance.IngameMainCamera.transform.position + (Vector3.up * 5.0f));
	}

	#region 메세지 처리

	public void OnMouseClickThis()
	{
		iTween.ScaleTo(gameObject,
			iTween.Hash("scale", Vector3.one * 2.0f, "easetype", "easeOutQuart", "time", 0.3f, "ignoretimescale", true));
	}

	public void OnMouseClickOther()
	{
		iTween.ScaleTo(gameObject,
			iTween.Hash("scale", Vector3.zero, "easetype", "easeInQuart", "time", 0.3f, "ignoretimescale", true));
	}

	#endregion

}
