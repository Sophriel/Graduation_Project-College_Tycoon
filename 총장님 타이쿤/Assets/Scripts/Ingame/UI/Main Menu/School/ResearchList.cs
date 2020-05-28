using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchList : MonoBehaviour
{
	public Transform Layout;
	public GameObject ResearchPrefab;

	private List<GameObject> researchList = new List<GameObject>();
	private List<Research> researchInProgress = new List<Research>();

	public delegate void ResearchStartEvent();
	public event ResearchStartEvent OnResearchStart;

	public delegate void ResearchCompleteEvent(Research research);
	private event ResearchCompleteEvent onResearchComplete;

	#region 갱신

	void OnEnable()
    {
		UpdateProfessors();
    }

	public void UpdateProfessors()
	{
		while (researchList.Count > 0)
		{
			Destroy(researchList[0]);
			researchList.RemoveAt(0);
		}

		if (DeptManager.Instance.GetEstablishedCollege())
		{
			for (int i = 0; i < 3 - researchInProgress.Count; i++)
			{
				researchList.Add(Instantiate(ResearchPrefab, Layout));
			}
		}
	}

	#endregion

	#region 진행

	public void OnClickStart(Research research)
	{
		researchInProgress.Add(research);
		researchList.Remove(research.gameObject);

		OnResearchStart?.Invoke();
		OnResearchStart = null;
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < researchInProgress.Count; i++)
			if (researchInProgress[i].UpdateProgress())
			{
				RemoveCompleted(researchInProgress[i]);
				i--;
			}
	}

	public void RemoveCompleted(Research research)
	{
		researchInProgress.Remove(research);
		Destroy(research.gameObject);
	}

	#endregion
}
