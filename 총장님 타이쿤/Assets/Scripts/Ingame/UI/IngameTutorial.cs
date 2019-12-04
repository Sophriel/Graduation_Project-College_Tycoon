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

    public Transform CameraPos;


    void Start()
    {
        anim = GetComponent<Animator>();

        Text = GetComponentInChildren<TextMeshPro>();
        isDialogBoxOpen = false;
        DialogFlag = false;
        DialogBox.SetActive(false);

        StartCoroutine(Tutorial());
    }

    private void Update()
    {
        DialogBox.transform.LookAt(CameraPos.position);
    }

    IEnumerator Tutorial()
    {
        //  이동
        //  대화
        //  카메라 움직임
        //  대화
        //  오른쪽 마우스
        //  대화
        //  건물 패널 열고
        //  대화
        //  건물 선택해서 짓고
        //  대화

        //  말풍선 새로운거
        //  사운드, 배경음 추가



        anim.SetBool("Point", false);
        DialogBox.SetActive(true);
        Text.text = "";
        yield return new WaitForSeconds(2.0f);
        OnOffDialog();
        yield return new WaitForSeconds(1.0f);

        Text.text = "여기가 튜토리얼 맵입니댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "먼저 카메라를 움직여봅시댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "WASD로 왔다갔다합니댱! \n";
        yield return new WaitUntil(ContinueDialog);
        yield return new WaitForSeconds(3.0f);
        SetDialogFlag(false);

        Text.text = "그 다음 줌을 당겨봅시댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "마우스 휠로 저에게 가까이 와보라냥! \n";
        yield return new WaitUntil(ContinueDialog);
        SetDialogFlag(false);
        yield return new WaitForSeconds(2.0f);

        Text.text = "으앗 너무 가깝습니댱! \n";
        yield return new WaitForSeconds(2.0f);
        //  뒤로 이동


        Text.text = "Q E 키로 빙글빙글 돌 수 있고 \n";
        yield return new WaitForSeconds(2.0f);
        Text.text += "방향키 위아래로 끄덕끄덕 할 수 있습니댱! \n";
        yield return new WaitUntil(ContinueDialog);
        yield return new WaitForSeconds(3.0f);
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

    private void WASDCheck()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") > 0f)
            SetDialogFlag(true);
    }

    private void ZoomCheck()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            SetDialogFlag(true);
    }

    private void RotationCheck()
    {
        if (Input.GetAxis("Mouse X") > 0f || Input.GetAxis("Mouse Y") > 0f)
            SetDialogFlag(true);
    }

}
