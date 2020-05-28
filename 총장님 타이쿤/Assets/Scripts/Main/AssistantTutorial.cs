using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssistantTutorial : MonoBehaviour
{
    private Animator anim;

    public GameObject DialogBox;
    private bool isDialogBoxOpen;
    public bool DialogFlag;
    public TextMeshPro Text;
    private float dialogSpeed = 1.5f;

    public GameObject AnswerUI;
    //public GameObject RegionUI;
    public GameObject LandUI;


    void Start()
    {
        anim = GetComponent<Animator>();

        Text = GetComponentInChildren<TextMeshPro>();
        isDialogBoxOpen = false;
        DialogFlag = false;
        DialogBox.SetActive(false);

        StartCoroutine(Tutorial());
    }

	#region 튜토리얼 진행

	IEnumerator Tutorial()
    {
        SoundManager.Instance.FadeOut(15, 10, 3.0f);
        DialogBox.SetActive(true);
        Text.text = "";
        yield return new WaitForSeconds(2.0f);
        OnOffDialog();
        yield return new WaitForSeconds(1.0f);

        Text.text = "안녕하세요 총쟝님! \n";
        yield return new WaitForSeconds(dialogSpeed);
        Text.text += "천국에 오신것을 환영합니댱! \n";
        yield return new WaitForSeconds(dialogSpeed);
        for (int i = 0; i < 3; i++)
        {
            Text.text += ".";
            yield return new WaitForSeconds(0.5f);
        }

        Text.text = "는 장난이댱... \n";
        yield return new WaitForSeconds(dialogSpeed);
        Text.text += "취임하신것을 축하드립니댱! \n";
        yield return new WaitForSeconds(dialogSpeed);
        Text.text += "저는 총쟝님의 비서 냥비서입니댱!";
        yield return new WaitForSeconds(dialogSpeed);

        Text.text = "지금부터 새로운 대학의 설립을 \n";
        yield return new WaitForSeconds(dialogSpeed);
        Text.text += "도와드리겠습니댱! \n";
        yield return new WaitForSeconds(dialogSpeed);
        Text.text += "준비되셨냐옹? ";
        anim.SetBool("Point", true);
        yield return new WaitForSeconds(dialogSpeed);
        OpenTutorialUI(AnswerUI);
        anim.SetBool("Point", false);
        yield return new WaitUntil(ContinueDialog);
        SetDialogFlag(false);
        CloseTutorialUI(AnswerUI);

        OnOffDialog();
        Text.text = "";
        MoveUp();
        yield return new WaitForSeconds(2.0f);

        OnOffDialog();
        Text.text = "먼저 설립하실 지역을 정하셔야한댱! \n";
        yield return new WaitForSeconds(dialogSpeed);
        //OpenTutorialUI(RegionUI);
        //yield return new WaitForSeconds(2.0f);
        //Text.text += "아직은 돈이 없어서 충청도에 가야한댱... \n";
        //yield return new WaitUntil(ContinueDialog);
        //SetDialogFlag(false);
        //Text.text = "그 다음 설립할 부지를 선정하셔야한댱! \n";
        //yield return new WaitForSeconds(3.0f);

        OpenTutorialUI(LandUI);
        yield return new WaitForSeconds(dialogSpeed);
        Text.text = "새로 오신 총쟝님을 위한 \n";
        yield return new WaitForSeconds(dialogSpeed);
        Text.text += "튜토리얼 맵이 있댱! \n";
        yield return new WaitForSeconds(dialogSpeed);
        Text.text += "선택해보시라냥! \n";
        yield return new WaitUntil(ContinueDialog);
        SetDialogFlag(false);
        CloseTutorialUI(LandUI);

        Text.text = "좋습니댱! 그럼 출발합니댱! \n";
        SoundManager.Instance.FadeOut(10, 0, 3.0f);
        yield return new WaitForSeconds(dialogSpeed);
        OnOffDialog();
        yield return new WaitForSeconds(dialogSpeed);

        StartCoroutine(MainManager.Instance.ChangeScene("Ingame"));
    }

    private void OnOffDialog()
    {
        //  off
        if (isDialogBoxOpen)
        {
            iTween.ScaleTo(DialogBox, iTween.Hash("scale", Vector3.zero));
            isDialogBoxOpen = false;
        }
        //  on
        else
        {
            iTween.ScaleTo(DialogBox, iTween.Hash("scale", Vector3.one));
            isDialogBoxOpen = true;
        }
    }

    ///<summary> 버튼 클릭시 true로 set </summary>  
    public void SetDialogFlag(bool input)
    {
        DialogFlag = input;
    }

    ///<summary> 대화를 진행하는 함수. UI튜토리얼 진행시 호출 </summary>  
    public bool ContinueDialog()
    {
        return DialogFlag;
    }

	#endregion

	#region UI 작용

	public void OpenTutorialUI(GameObject ui)
    {
        ui.transform.localScale = Vector3.zero;

        if (!ui.activeInHierarchy)
            ui.SetActive(true);

        iTween.ScaleTo(ui, iTween.Hash("scale", Vector3.one, "time", 0.7f, "easetype", "easeOutBack"));
    }

    public void CloseTutorialUI(GameObject ui)
    {
        //  왜 안꺼질까
        iTween.ScaleTo(ui, iTween.Hash("scale", Vector3.zero, "time", 0.7f, "easetype", "easeInBack", 
            "oncomplete", "SetActive", "oncompletetarget", ui, "oncompleteparams", false));
    }

    private void MoveUp()
    {
        iTween.MoveAdd(gameObject, Vector3.up * 3f, 1.5f);
    }

	#endregion
}
