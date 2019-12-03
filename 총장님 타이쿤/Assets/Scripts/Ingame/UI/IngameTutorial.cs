using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameTutorial : MonoBehaviour
{
    private Animator anim;

    public GameObject DialogBox;
    private bool isDialogBoxOpen;
    public bool DialogFlag;
    public TextMeshPro Text;


    void Start()
    {
        anim = GetComponent<Animator>();

        Text = GetComponentInChildren<TextMeshPro>();
        isDialogBoxOpen = false;
        DialogFlag = false;
        DialogBox.SetActive(false);

        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        DialogBox.SetActive(true);
        Text.text = "";
        yield return new WaitForSeconds(2.0f);
        OnOffDialog();
        yield return new WaitForSeconds(1.0f);

        Text.text = "여기가 튜토리얼 맵입니댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "천국에 오신것을 환영합니댱! \n";
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 3; i++)
        {
            Text.text += ".";
            yield return new WaitForSeconds(0.5f);
        }

        anim.SetBool("Point", false);
        yield return new WaitUntil(ContinueDialog);
        SetDialogFlag(false);

        OnOffDialog();
        Text.text = "";
        MoveUp();
        yield return new WaitForSeconds(2.0f);

        SetDialogFlag(false);
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
}
