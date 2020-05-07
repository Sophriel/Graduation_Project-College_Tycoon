using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface SwitchableUI
{
	void OnClick();
}

public class GameManager : MonoBehaviour
{
	#region Singleton

	private static volatile GameManager instance;
	private static object _lock = new System.Object();

	public static GameManager Instance
	{
		get
		{
			if (instance != null)
				return instance;

			instance = FindObjectOfType<GameManager>();

			if (instance != null)
				return instance;

			CreateThis();

			return instance;
		}
	}

	public static GameManager CreateThis()
	{
		GameObject MouseManagerGameObject = new GameObject("Dept Manager");

		//  하나의 스레드로만 접근 가능하도록 lock
		lock (_lock)
			instance = MouseManagerGameObject.AddComponent<GameManager>();

		return instance;
	}

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
	}

	#endregion

	public GameObject IngameMainCamera;

	public GameObject Fade;
	private Image fadeImage;

	void Start()
	{
		fadeImage = Fade.GetComponent<Image>();

		Fade.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		iTween.ValueTo(gameObject, iTween.Hash("from", 255f, "to", 0f, "time", 1.5f,
			"easetype", "easeInCubic", "onupdate", "FadeUpdate", "oncomplete", "FadeComplete"));

		SoundManager.Instance.FadeOut(0, 20, 5.0f);

		buildingsInGame = new List<GameObject>();
		buildingsInGame.AddRange(GameObject.FindGameObjectsWithTag("Building"));
	}

	#region 페이딩

	private void FadeUpdate(int alpha)
	{
		fadeImage.color = new Color(1f, 1f, 1f, alpha / 255f);
	}

	private void FadeComplete()
	{
		Fade.SetActive(false);
	}

	#endregion

	#region 게임 내 건물 관리

	private List<GameObject> buildingsInGame;

	public GameObject GetRandomBuildingInGame()
	{
		if (buildingsInGame.Count > 0)
			return buildingsInGame[Random.Range(0, buildingsInGame.Count)];

		return null;
	}

	public void AddBuildingInGame(GameObject building)
	{
		if (building.CompareTag("Building"))
			buildingsInGame.Add(building);
	}

	public void DeleteBuildingInGame(GameObject building)
	{
		if (buildingsInGame.Contains(building))
			buildingsInGame.Remove(building);
	}

	#endregion

	#region UI 관리

	private SwitchableUI uiOnScreen;

	public void RegisterUI(SwitchableUI ui)
	{
		CloseUI();

		uiOnScreen = ui;
	}

	public void CancleUI(SwitchableUI ui)
	{
		if (uiOnScreen == ui)
			uiOnScreen = null;
	}

	public void CloseUI()
	{
		if (uiOnScreen != null)
			uiOnScreen.OnClick();
	}

	#endregion

}
