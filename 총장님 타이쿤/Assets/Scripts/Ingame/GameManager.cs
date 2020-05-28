using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	}

	#endregion

	public GameObject IngameMainCamera;
	public IngameTutorial Assistant;
	public ResultPanel Result;

	public GameObject Fade;
	private Image fadeImage;

	void Start()
	{
		fadeImage = Fade.GetComponent<Image>();

		Fade.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		iTween.ValueTo(gameObject, iTween.Hash("from", 255f, "to", 0f, "time", 1.5f,
			"easetype", "easeInCubic", "onupdate", "FadeUpdate", "oncomplete", "FadeComplete", "ignoretimescale", true));

		SoundManager.Instance.FadeOut(0, 10, 5.0f);

		buildingsInGame = new List<GameObject>();
		buildingsInGame.AddRange(GameObject.FindGameObjectsWithTag("Building"));

		SetMoney();
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

	#region 냥비서님 스크립트

	public void StartSemester()
	{
		StartCoroutine(Assistant.StartSemester());
	}

	public void EndSemester()
	{
		PeopleManager.Instance.EvaluateSchool();
		DeptManager.Instance.EvaluateSchool();

		FinancialPoint = (int)(2000.0 / System.Math.Sqrt(System.Math.PI * 2) * System.Math.Pow(System.Math.E, System.Math.Pow((Money - 200000.0) / 500000.0, 2.0) / -2));
		SchoolPoint = (ResearchPoint + TeachPoint) * (FamePoint / 10) + SatisfactionPoint + FinancialPoint;

		StartCoroutine(Assistant.EndSemester());
	}

	public void EvaluateSchool()
	{
		PeopleManager.Instance.EvaluateSchool();
		DeptManager.Instance.EvaluateSchool();

		FinancialPoint = (int)(2000.0 / System.Math.Sqrt(System.Math.PI * 2) * System.Math.Pow(System.Math.E, System.Math.Pow((Money - 200000.0) / 500000.0, 2.0) / -2));
		SchoolPoint = (ResearchPoint + TeachPoint) * (FamePoint / 10) + SatisfactionPoint + FinancialPoint;

		string schoolGrade = "F";

		if (SchoolPoint > 20000)
			schoolGrade = "A";
		else if (SchoolPoint > 15000)
			schoolGrade = "B";
		else if (SchoolPoint > 10000)
			schoolGrade = "C";
		else if (SchoolPoint > 5000)
			schoolGrade = "D";

		StartCoroutine(Result.EvaluateSchool(schoolGrade));
	}

	#endregion

	#region 학교 평가

	public int ResearchPoint = 0;
	public int TeachPoint = 0;
	public int SatisfactionPoint = 0;
	public int FamePoint = 0;
	public int FinancialPoint = 0;

	public int SchoolPoint = 0;

	#endregion

	#region 자금

	public int Money { get; private set; }
	public TextMeshProUGUI MoneyText;

	private void SetMoney()
	{
		Money = 50000;
		MoneyText.text = string.Format("{0:#,###}", Money);
	}

	public void EarnMoney(int money)
	{
		Money += money;
		MoneyText.text = string.Format("{0:#,###}", Money);
	}

	public bool CanSpendMoney(int price)
	{
		if (Money >= price)
		{
			return true;
		}

		else
		{
			StartCoroutine(Assistant.NeedMoreMoney(price - Money));
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
