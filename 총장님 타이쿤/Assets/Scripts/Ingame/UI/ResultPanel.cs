using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
	public bool Is_Open = false;
	public TextMeshProUGUI ResearchPoint;
	public TextMeshProUGUI TeachPoint;
	public TextMeshProUGUI SatisfactionPoint;
	public TextMeshProUGUI FinancialPoint;

	public TextMeshProUGUI SchoolPoint;
	public TextMeshProUGUI Grade;

	#region OnClick 구현

	public void OnClick()
	{
		SoundManager.Instance.PlayEffect((int)SoundManager.Effect.MenuOpen);

		//  열기
		if (!Is_Open)
		{
			OpenResultPanel();
		}

		//  닫기
		else
		{
			CloseResultPanel();
		}
	}

	public void OpenResultPanel()
	{
		//  작용
		Is_Open = true;
		ScheduleManager.Instance.GameTime.OnPauseButton();

		//  연출
		gameObject.SetActive(true);
		iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "easetype", "easeOutQuart", "time", 0.3f, "ignoretimescale", true));
	}

	public void CloseResultPanel()
	{
		//  작용
		Is_Open = false;
		ScheduleManager.Instance.GameTime.OnPlayButton();

		//  연출
		iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "easetype", "easeInQuart", "time", 0.3f, "oncomplete", "DisableMenu", "ignoretimescale", true));
	}

	private void DisableMenu()
	{
		gameObject.SetActive(false);
	}

	#endregion

	public void MoveToMainMenu()
	{
		ScheduleManager.Instance.GameTime.OnPlayButton();
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
	}

	public IEnumerator EvaluateSchool(string grade)
	{
		OpenResultPanel();

		FinancialPoint.text = "재정: ";
		TeachPoint.text = "수업: ";
		SatisfactionPoint.text = "만족도: ";
		ResearchPoint.text = "연구: ";
		SchoolPoint.text = "총 점: ";
		Grade.text = "";

		yield return new WaitForSecondsRealtime(2.0f);
		FinancialPoint.text += GameManager.Instance.FinancialPoint.ToString();
		yield return new WaitForSecondsRealtime(2.0f);
		TeachPoint.text += GameManager.Instance.TeachPoint.ToString();
		yield return new WaitForSecondsRealtime(2.0f);
		SatisfactionPoint.text += GameManager.Instance.SatisfactionPoint.ToString();
		yield return new WaitForSecondsRealtime(2.0f);
		ResearchPoint.text += GameManager.Instance.ResearchPoint.ToString();
		yield return new WaitForSecondsRealtime(2.0f);

		SchoolPoint.text += GameManager.Instance.SchoolPoint.ToString();
		yield return new WaitForSecondsRealtime(2.0f);
		Grade.text = grade;
		yield return new WaitForSecondsRealtime(2.0f);
	}
}
