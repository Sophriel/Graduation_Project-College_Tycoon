using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface SwitchableUI
{
	void OnClick();
}

//학교 점수 = 연구 포인트 + 수업 포인트 + 욕구 포인트 + 명성 가중치 + 재정 상태
//	각 학과마다 연구 생성하고 수주받기
//	사람들의 욕구 이벤트
//  건물 적거나 할당이 안되어있으면 욕구점수 까임
//	학교 점수로 학교 평가

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

		SetMoney();
	}

	private void Update()
	{
		SchoolPoint = (ResearchPoint + TeachPoint) * (FamePoint / 10) + SatisfactionPoint + FinancialPoint;
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

	#region 학교 평가

	public int ResearchPoint;
	public int TeachPoint;
	public int SatisfactionPoint;
	public int FamePoint;
	public int FinancialPoint;

	public int SchoolPoint = 0;

	#endregion

	#region 자금

	public int Money { get; private set; }
	public TextMeshProUGUI MoneyText;

	private void SetMoney()
	{
		Money = 500000;
		MoneyText.text = string.Format("{0:#,###}", Money);
	}

	public void EarnMoney(int money)
	{
		Money += money;
		MoneyText.text = string.Format("{0:#,###}", Money);
	}

	public bool CanSpendMoney(int price)
	{
		if (Money > price)
		{
			return true;
		}

		else
		{
			return false;
		}
	}

	public void SpendMoney(int price)
	{
		Money -= price;
		MoneyText.text = string.Format("{0:#,###}", Money);
	}



	#endregion
}
