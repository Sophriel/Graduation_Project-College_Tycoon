using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Fade;
    private Image fadeImage;

    void Start()
    {
        fadeImage = Fade.GetComponent<Image>();

        Fade.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        iTween.ValueTo(gameObject, iTween.Hash("from", 255f, "to", 0f, "time", 2.5f,
            "easetype", "easeOutCubic", "onupdate", "FadeUpdate"));

    }

    private void FadeUpdate(int alpha)
    {
        fadeImage.color = new Color(1f, 1f, 1f, alpha / 255f);
    }

    void Update()
    {
        
    }
}
