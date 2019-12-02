using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public GameObject Cam;
    private iTweenPath itPath;
    public GameObject Fade;
    private Image fadeImage;

    public GameObject CatAssistant;

    //  캔버스
    public RectTransform _UI;

    //  타이틀 텍스트
    public TextMeshProUGUI Title;
    public TextMeshProUGUI GameStart;
    public TextMeshProUGUI QuitGame;

    public TextMeshProUGUI Tutorial;
    public TextMeshProUGUI NewGame;
    public TextMeshProUGUI LoadGame;
    public TextMeshProUGUI Back;


    private void Start()
    {
        itPath = GetComponent<iTweenPath>();
        fadeImage = Fade.GetComponent<Image>();

        StartCoroutine(MainCoroutine());
    }

    #region 페이드

    IEnumerator MainCoroutine()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 255f, "time", 5f, "easetype", iTween.EaseType.linear,
            "onupdate", "FadeTitle"));

        yield return new WaitForSeconds(3.5f);

        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 255f, "time", 2f, "easetype", iTween.EaseType.linear,
           "onupdate", "FadeStart"));

        yield return new WaitForSeconds(1.0f);

        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 255f, "time", 2f, "easetype", iTween.EaseType.linear,
            "onupdate", "FadeQuit"));

        yield break;
    }

    private void FadeUpdate(int alpha)
    {
        fadeImage.color = new Color(1f, 1f, 1f, alpha / 255f);
    }
    private void FadeTitle(int alpha)
    {
        Title.alpha = alpha / 255.0f;
    }
    private void FadeStart(int alpha)
    {
        GameStart.alpha = alpha / 255.0f;
    }
    private void FadeQuit(int alpha)
    {
        QuitGame.alpha = alpha / 255.0f;
    }

    #endregion

    #region 버튼 리액션

    public void GameStartButton()
    {
        iTween.MoveTo(_UI.gameObject, iTween.Hash("x", -Screen.width / 2f));
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void TutorialButton()
    {
        iTween.MoveTo(_UI.gameObject, iTween.Hash("x", -Screen.width * 2));

        iTween.MoveTo(Cam, iTween.Hash("path", iTweenPath.GetPath("CameraPath"), "orienttopath", true,
            "time", 5f, "easetype", "easeInCubic"));

        Fade.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 255f, "time", 5f,
            "easetype", "easeInCubic", "onupdate", "FadeUpdate", "oncomplete", "StartTutorial"));
    }

    public void NewGameButton()
    {

    }

    public void LoadGameButton()
    {

    }

    public void BackButton()
    {
        iTween.MoveTo(_UI.gameObject, iTween.Hash("x", Screen.width / 2f));
    }

    private void StartTutorial()
    {
        CatAssistant.SetActive(true);

        iTween.ValueTo(gameObject, iTween.Hash("from", 255f, "to", 0f, "time", 3f,
          "easetype", "easeInQuad", "onupdate", "FadeUpdate"));
    }

    #endregion

    private void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
