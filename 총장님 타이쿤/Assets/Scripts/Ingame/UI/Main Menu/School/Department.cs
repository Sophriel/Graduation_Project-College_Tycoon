using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Department : MonoBehaviour
{
	protected Color DeepColor = new Color(0.0f, 46.0f / 255.0f, 197.0f / 255.0f, 1.0f);
	protected Color FaintColor = new Color(0.0f, 46.0f / 255.0f, 197.0f / 255.0f, 100 / 255.0f);

	public TextMeshProUGUI KorName;
	public bool IsEstablished { get; protected set; }

	protected string info;
	public List<GameObject> AssignedBuilding = new List<GameObject>();

	protected string[] researches;

	public virtual void OnClick() { }
	public virtual void SetActiveThis() { }
	public virtual bool Establish() { return true; }
	public virtual void AssignBuilding() { }
	public virtual void Seminar() { }
	public virtual void Close() { }
}
