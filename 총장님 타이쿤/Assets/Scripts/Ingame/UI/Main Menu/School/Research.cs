using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Research : MonoBehaviour
{
	public string ResearchName;
	public TextMeshProUGUI NameText;
	public College CollegeInCharge;
	public TextMeshProUGUI CollegeText;

	public int Price;
	public TextMeshProUGUI PriceText;
	public int RP;
	public TextMeshProUGUI RPText;

	private bool is_Progressing = false;
	private float researcher;
	public float Progress = 0;
	public TextMeshProUGUI ProgressText;

	void Start()
    {
		switch (Random.Range(0, 8))
		{
			case 0:
				ResearchName = "게임 과몰입과 중독에 영향을 미치는 게임 요소와 메커니즘 연구";
				break;

			case 1:
				ResearchName = "e스포츠 전문화 수준이 플로우(Flow)와 관람만족에 미치는 영향";
				break;

			case 2:
				ResearchName = "MMORPG에서 세계관의 변화 양상 연구";
				break;

			case 3:
				ResearchName = "스토리텔링 카드 게임 구현에 관한 연구";
				break;

			case 4:
				ResearchName = "플래시백을 이용한 VR 인터랙티브 스토리텔링 콘텐츠 연구";
				break;

			case 5:
				ResearchName = "블록체인을 활용한 온라인 게임 분석 연구";
				break;

			case 6:
				ResearchName = "VR게임의 실재감과 감정이입에 관한 연구";
				break;

			case 7:
				ResearchName = "개발엔진이 제공하는 랜덤 메소드 신뢰도에 대한 연구";
				break;

			default:
				break;
		}
		NameText.text = ResearchName;

		CollegeInCharge = DeptManager.Instance.GetEstablishedCollege();
		CollegeText.text = CollegeInCharge.KorName.text;

		Price = Random.Range(3000, 10000);
		PriceText.text = "$ " + Price.ToString();
		RP = Price / 10;
		RPText.text = RP.ToString() + " RP";
	}

	public void OnStart()
	{
		if (is_Progressing)
		{
			return;
		}

		GameManager.Instance.SpendMoney(Price);

		is_Progressing = true;
		GameManager.Instance.GetComponent<ResearchList>().OnClickStart(this);
	}

	public bool UpdateProgress()
	{
		if (!is_Progressing)
		{
			return false;
		}

		researcher = 0f;

		foreach (Major subDept in CollegeInCharge.SubDepts)
			foreach (Professor p in subDept.Professors)
				researcher += p.Researching;

		Progress += researcher / 1000.0f;
		ProgressText.text = "진행중..." + ((int)(Progress / RP * 100f)).ToString() + "%";

		if (Progress > RP)
		{
			is_Progressing = false;

			GameManager.Instance.ResearchPoint += RP;

			foreach (Major subDept in CollegeInCharge.SubDepts)
				foreach (Professor p in subDept.Professors)
					p.Teaching++;

			return true;
		}

		return false;
	}
}