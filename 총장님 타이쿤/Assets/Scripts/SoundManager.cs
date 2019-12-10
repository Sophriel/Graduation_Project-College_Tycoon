using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    private static volatile SoundManager instance;
    private static object _lock = new System.Object();

    public static SoundManager Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<SoundManager>();

            if (instance != null)
                return instance;

            CreateThis();

            return instance;
        }
    }

    public static SoundManager CreateThis()
    {
        GameObject MouseManagerGameObject = new GameObject("Sound Manager");

        //  하나의 스레드로만 접근 가능하도록 lock
        lock (_lock)
            instance = MouseManagerGameObject.AddComponent<SoundManager>();

        return instance;
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    public AudioSource BGMSource;
    public AudioSource EffectSource;
    public List<AudioClip> BGMs;
    public List<AudioClip> SoundEffects;

    public enum Effect
    {
        Click = 0,
        Cancel,
        MenuOpen,
        MenuClose,
        Build,
        Cant
    }

    #region BGM

    public void ChangeBGM(int num)
    {
        BGMSource.clip = BGMs[num];
        BGMSource.Play();
    }

    public void FadeIn(int start, int target, float time)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", start, "to", target, "time", time,
            "easetype", "easeInQuad", "onupdate", "FadeAudio"));
    }

    public void FadeOut(int start, int target, float time)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", start, "to", target, "time", time,
            "easetype", "easeInQuad", "onupdate", "FadeAudio"));
    }

    private void FadeAudio(int volume)
    {
        BGMSource.volume = volume / 100f;
    }

    #endregion

    public void PlayEffect(int effect)
    {
        EffectSource.PlayOneShot(SoundEffects[effect]);
    }
}
