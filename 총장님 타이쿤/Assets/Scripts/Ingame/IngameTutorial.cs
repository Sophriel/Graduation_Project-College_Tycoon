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
	public GameObject DeptPanel;
	public GameObject ProfessorPanel;

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

	#region 냥비서님 떠드는 스크립트

	IEnumerator Tutorial()
    {
		// Init
		SoundManager.Instance.ChangeBGM(3);
		anim.SetInteger("State", 0);
		DialogBox.SetActive(true);
		Text.text = "";
		yield return new WaitForSecondsRealtime(2.0f);
		OnOffDialog();
		yield return new WaitForSecondsRealtime(1.5f);

		#region 카메라 튜토

		//  대화
		Text.text = "여기가 튜토리얼 맵입니댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "먼저 카메라를 움직여봅시댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "WASD로 왔다갔다합니댱! \n";

		//  카메라 이동
		checker = WASDCheck;
		yield return new WaitUntil(ContinueDialog);
		yield return new WaitForSecondsRealtime(1.0f);
		SetDialogFlag(false);


		//  카메라 줌
		Text.text = "그 다음 줌을 당겨봅시댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "마우스 휠로 저에게 \n가까이 와보라냥! \n";

		checker = ZoomCheck;
		yield return new WaitUntil(ContinueDialog);
		SetDialogFlag(false);
		yield return new WaitForSecondsRealtime(2.0f);


		//  뒤로 이동
		Text.text = "으앗 너무 가깝습니댱! \n";
		anim.SetInteger("State", 2);
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("Tutorial"),
			"time", 2f, "easetype", "linear"));
		yield return new WaitForSecondsRealtime(2.0f);


		//  카메라 회전
		anim.SetInteger("State", 0);
		Text.text = "Q,E 키로 빙글빙글 돌 수 있고 \n";
		yield return new WaitForSecondsRealtime(2.0f);
		Text.text += "방향키 위아래로 끄덕끄덕 \n할 수 있습니댱! \n";

		checker = RotationCheck;
		yield return new WaitUntil(ContinueDialog);
		yield return new WaitForSecondsRealtime(2.0f);
		SetDialogFlag(false);

		#endregion

		#region 건설 생성 튜토

		//  오른쪽 마우스
		Text.text = "오른쪽 마우스를 꾹 눌러보셔량! \n";
		checker = RightClickCheck;
		yield return new WaitUntil(ContinueDialog);
		SetDialogFlag(false);


		//  건물 패널 열기
		Text.text = "망치버튼 위에서 \n마우스를 떼시면...! \n";
		checker = OpenPanelCheck;
		yield return new WaitUntil(ContinueDialog);
		yield return new WaitForSecondsRealtime(1.0f);
		SetDialogFlag(false);

		Text.text += "건설 패널이 열린댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);


		//  건물 선택해서 짓기
		Text.text = "첫 건물을 한번 지어봅시댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "빌딩 버튼을 눌러 \n마음에 드는 건물을 지어보셔량! \n";
		yield return new WaitForSecondsRealtime(2.5f);
		OnOffDialog();
		MouseManager.Instance.AddTutorialEvent(SetDialogFlag);
		yield return new WaitUntil(ContinueDialog);
		yield return new WaitForSecondsRealtime(0.5f);
		MouseManager.Instance.DeleteTutorialEvent(SetDialogFlag);
		SetDialogFlag(false);

		#endregion

		#region 학과 설립 튜토

		OnOffDialog();
		Text.text = "좋습니댱! 이제 새로운 학과를 \n설립해 볼 차례댱! \n";
		yield return new WaitForSecondsRealtime(2.0f);
		Text.text = "우클릭 메뉴를 통해 \n학교모양을 선택하셔량! \n";
		checker = OpenDeptPanelCheck;
		yield return new WaitUntil(ContinueDialog);
		SetDialogFlag(false);

		Text.text = "왼쪽의 학과 버튼을 클릭하고 \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "+버튼을 눌러 원하는 대학을 \n설립하셔량! ";
		DeptManager.Instance.AddTutorialEvent(SetDialogFlag);
		yield return new WaitUntil(ContinueDialog);
		DeptManager.Instance.DeleteTutorialEvent(SetDialogFlag);
		SetDialogFlag(false);

		Text.text += "그 다음 학과도 \n설립하시는거댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		DeptManager.Instance.AddTutorialEvent(SetDialogFlag);
		yield return new WaitUntil(ContinueDialog);
		yield return new WaitForSecondsRealtime(0.5f);
		DeptManager.Instance.DeleteTutorialEvent(SetDialogFlag);
		SetDialogFlag(false);

		GameManager.Instance.CloseUI();
		Text.text = "이제 교수님을 모셔볼 차례입니댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "우클릭 메뉴를 통해 \n사람모양을 선택하시고 \n";
		checker = OpenProfessorPanelCheck;
		yield return new WaitUntil(ContinueDialog);
		yield return new WaitForSecondsRealtime(0.5f);
		SetDialogFlag(false);

		Text.text += "교수 버튼을 클릭해 면접을 보셔량! ";
		checker = RecruitProfessor;
		yield return new WaitUntil(ContinueDialog);
		yield return new WaitForSecondsRealtime(0.5f);
		SetDialogFlag(false);

		GameManager.Instance.CloseUI();
		Text.text = "준비는 끝났습니댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);

		#endregion

		Text.text = "열심히 하셔서 올해 꼭! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "좋은 평가를 받으셔량! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "건투를 빕니댱!\n";
		yield return new WaitForSecondsRealtime(2.5f);
		OnOffDialog();

		anim.SetInteger("State", 2);
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("EndTutorial"),
			"speed", 10.0f, "easetype", "linear"));
		yield return new WaitForSecondsRealtime(12.0f);
		anim.SetInteger("State", 0);

		ScheduleManager.Instance.EndTutorial();
	}

	public IEnumerator NeedMoreMoney(int money)
	{
		OnOffDialog();
		Text.text = "돈이 $" + money +" 더 필요합니댱!\n";
		yield return new WaitForSecondsRealtime(2.5f);
		OnOffDialog();
	}

	public IEnumerator StartSemester()
	{
		OnOffDialog();
		Text.text = "이제 새 학기가 시작됩니댱!\n";
		yield return new WaitForSecondsRealtime(2.5f);
		Text.text += "엄청 설렌다냥! \n";
		yield return new WaitForSecondsRealtime(2.5f);
		OnOffDialog();
	}

	public IEnumerator EndSemester()
	{
		OnOffDialog();
		Text.text = "종강했습니댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);
		Text.text += "현재까지 점수를 불러드리겠습니댱! \n";
		yield return new WaitForSecondsRealtime(1.5f);

		Text.text = "재정 점수: " + GameManager.Instance.FinancialPoint.ToString();
		yield return new WaitForSecondsRealtime(1.0f);
		Text.text += " 연구 점수: " + GameManager.Instance.ResearchPoint.ToString();
		yield return new WaitForSecondsRealtime(1.0f);
		Text.text += "\n수업 점수: " + GameManager.Instance.TeachPoint.ToString();
		yield return new WaitForSecondsRealtime(1.0f);
		Text.text += " 만족도 점수: " + GameManager.Instance.SatisfactionPoint.ToString();
		yield return new WaitForSecondsRealtime(1.0f);

		Text.text += "\n총 점: " + GameManager.Instance.SchoolPoint.ToString();
		yield return new WaitForSecondsRealtime(2.0f);
		OnOffDialog();
	}

	#endregion

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

	public void OpenDeptPanelCheck()
	{
		if (DeptPanel.activeInHierarchy)
			SetDialogFlag(true);
	}

	public void OpenProfessorPanelCheck()
	{
		if (ProfessorPanel.activeInHierarchy)
			SetDialogFlag(true);
	}

	public void RecruitProfessor()
	{
		if (PeopleManager.Instance.PeopleInGame.transform.childCount > 0)
			SetDialogFlag(true);
	}

	#endregion
}
