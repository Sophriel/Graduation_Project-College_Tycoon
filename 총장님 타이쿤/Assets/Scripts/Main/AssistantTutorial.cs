using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssistantTutorial : MonoBehaviour
{
    private Animator anim;

    public GameObject DialogBox;
    private bool isDialogBoxOpen;
    public TextMeshPro Text;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Point", true);

        Text = GetComponentInChildren<TextMeshPro>();
        isDialogBoxOpen = true;

        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        //Text.text = "";
        //yield return new WaitForSeconds(2.0f);

        //Text.text = "안녕하세요 총쟝님! \n";
        //yield return new WaitForSeconds(1.5f);
        //Text.text += "천국에 오신것을 환영합니댱! \n";
        //yield return new WaitForSeconds(1.5f);
        //for (int i = 0; i < 3; i++)
        //{
        //    Text.text += ".";
        //    yield return new WaitForSeconds(0.5f);
        //}

        //Text.text = "는 장난이댱... \n";
        //yield return new WaitForSeconds(1.5f);
        //Text.text += "취임하신것을 축하드립니댱! \n";
        //yield return new WaitForSeconds(1.5f);
        //Text.text += "저는 총장님의 비서 냥비서입니댱!";
        //yield return new WaitForSeconds(2.0f);

        Text.text = "지금부터 새로운 대학의 설립을 \n도와드리겠습니댱! \n";
        yield return new WaitForSeconds(2.0f);
        Text.text += "준비되셨냐옹? ";
        yield return new WaitForSeconds(1.0f);
        Text.text += " ...";
        yield return new WaitUntil(ContinueDialog);

        OnOffDialog();
        Text.text = "";
        MoveUp();
        yield return new WaitForSeconds(2.0f);

        OnOffDialog();
        Text.text += "먼저 설립하실 위치를 정하셔야한댱! \n";
        yield return new WaitForSeconds(1.5f);


        yield return new WaitForSeconds(2.0f);

        Text.text = "";
        yield return new WaitForSeconds(3.0f);
        Text.text += "취임하신것을 축하드립니다! \n";
        yield return new WaitForSeconds(1.5f);
        Text.text += "저는 총장님의 비서 냥비서입니다!";
        yield return new WaitForSeconds(2.0f);
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

    public bool ContinueDialog()
    {
        if (Input.anyKeyDown)
            return true;

        return false;
    }

    private void MoveUp()
    {
        iTween.MoveAdd(gameObject, Vector3.up * 3f, 1.5f);
    }
}
