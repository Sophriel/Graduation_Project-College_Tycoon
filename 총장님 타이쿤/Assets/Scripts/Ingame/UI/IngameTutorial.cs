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
    public GameObject BuildMenuBar;

    public Transform CameraPos;
    private delegate void TutorialCheckPoint();
    private TutorialCheckPoint checker;

    private iTweenPath itPath;

    void Start()
    {
        anim = GetComponent<Animator>();

        Text = GetComponentInChildren<TextMeshPro>();
        isDialogBoxOpen = false;
        DialogFlag = false;
        DialogBox.SetActive(false);

        checker = null;

        itPath = GetComponent<iTweenPath>();

        StartCoroutine(Tutorial());
    }

    private void Update()
    {
        DialogBox.transform.LookAt(CameraPos.position + (Vector3.down * 10f));

        checker?.Invoke();
    }

    IEnumerator Tutorial()
    {
        //  말풍선 새로운거
        //  사운드, 배경음 추가

        // Init
        SoundManager.Instance.ChangeBGM(3);
        anim.SetInteger("State", 0);
        DialogBox.SetActive(true);
        Text.text = "";
        yield return new WaitForSeconds(2.0f);
        OnOffDialog();
        yield return new WaitForSeconds(1.5f);


        //  대화
        Text.text = "여기가 튜토리얼 맵입니댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "먼저 카메라를 움직여봅시댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "WASD로 왔다갔다합니댱! \n";

        //  카메라 이동
        checker = WASDCheck;
        yield return new WaitUntil(ContinueDialog);
        yield return new WaitForSeconds(1.0f);
        SetDialogFlag(false);


        //  카메라 줌
        Text.text = "그 다음 줌을 당겨봅시댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "마우스 휠로 저에게 \n가까이 와보라냥! \n";

        checker = ZoomCheck;
        yield return new WaitUntil(ContinueDialog);
        SetDialogFlag(false);
        yield return new WaitForSeconds(2.0f);


        //  뒤로 이동
        Text.text = "으앗 너무 가깝습니댱! \n";
        anim.SetInteger("State", 2);
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("Tutorial"),
            "time", 2f, "easetype", "linear"));
        yield return new WaitForSeconds(2.0f);


        //  카메라 회전
        anim.SetInteger("State", 0);
        Text.text = "Q,E 키로 빙글빙글 돌 수 있고 \n";
        yield return new WaitForSeconds(2.0f);
        Text.text += "방향키 위아래로 끄덕끄덕 \n할 수 있습니댱! \n";

        checker = RotationCheck;
        yield return new WaitUntil(ContinueDialog);
        yield return new WaitForSeconds(2.0f);
        SetDialogFlag(false);


        //  오른쪽 마우스
        Text.text = "오른쪽 마우스를 꾹 눌러보셔량! \n";
        checker = RightClickCheck;
        yield return new WaitUntil(ContinueDialog);
        SetDialogFlag(false);


        //  건물 패널 열기
        Text.text = "망치버튼 위에서 \n마우스를 떼시면...! \n";
        checker = OpenPanelCheck;
        yield return new WaitUntil(ContinueDialog);
        yield return new WaitForSeconds(1.0f);
        SetDialogFlag(false);

        Text.text += "건설 패널이 열린댱! \n";
        yield return new WaitForSeconds(1.5f);


        //  건물 선택해서 짓기
        Text.text = "첫 건물을 한번 지어봅시댱! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "건물 버튼을 눌러 \n 마음에 드는 건물을 지어보셔량! \n";
        yield return new WaitForSeconds(2.5f);
        OnOffDialog();
        MouseManager.Instance.SetMouseEvent(new MouseManager.MouseEvent(SetDialogFlag));
        yield return new WaitUntil(ContinueDialog);
        yield return new WaitForSeconds(1.0f);

        SetDialogFlag(false);
        OnOffDialog();
        Text.text = "튜토리얼은 여기까집니댱! \n";
        MouseManager.Instance.ClearMouseEvent();
        yield return new WaitForSeconds(2.5f);
        OnOffDialog();
    }

    #region 튜토리얼 UI 조작

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
            iTween.ScaleTo(DialogBox, iTween.Hash("scale", Vector3.one * 2));
            isDialogBoxOpen = true;
        }
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

    #endregion

    #region 튜토리얼 체크포인트

    ///<summary> 버튼 클릭시 true로 set </summary>  
    public void SetDialogFlag(bool input)
    {
        DialogFlag = input;

        checker = null;
    }

    ///<summary> 대화를 진행하는 함수. UI튜토리얼 진행시 호출 </summary>  
    public bool ContinueDialog()
    {
        return DialogFlag;
    }

    private void WASDCheck()
    {
        if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Vertical") > 0f)
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

    private void RightClickCheck()
    {
        if (Input.GetButtonDown("RightClick"))
            SetDialogFlag(true);
    }

    public void OpenPanelCheck()
    {
        if (BuildMenuBar.activeInHierarchy)
            SetDialogFlag(true);
    }

    #endregion
}
