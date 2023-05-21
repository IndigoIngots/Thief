using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class HomeController : MonoBehaviour
{
    [SerializeField] Image FadePanel;
    [SerializeField] GameObject HowToPanel;

    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SE;

    [SerializeField] AudioClip Slide;

    // Start is called before the first frame update
    void Start()
    {
        FadePanel.color = new Color(0f, 0f, 0f, 1f);
        Invoke("PushFadePanel", 1.0f);
        Invoke("StartBGM", 1.0f);
    }

    public void PushFadePanel()
    {
        DOTween.ToAlpha(
        () => FadePanel.color,
        color => FadePanel.color = color,
        0f, 1f);
        Invoke("falseFadePanel", 1f);
    }

    public void falseFadePanel()
    {
        FadePanel.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        StartFade();

        StartCoroutine("VolumeDown");

        Invoke("ChangeScene", 1.0f);

        SE.PlayOneShot(Slide);
    }

    public void ChangeScene()
    { 
        SceneManager.LoadScene("Main");
    }

    public void StartFade()
    {
        FadePanel.gameObject.SetActive(true);

        FadePanel.color = new Color(0f, 0f, 0f, 0f);
        DOTween.ToAlpha(
        () => FadePanel.color,
        color => FadePanel.color = color,
        1f, 1f);
    }

    public void StartBGM()
    {
        BGM.Play();
    }

    public void PushHowTo()
    {
        HowToPanel.SetActive(true);
        HowToPanel.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0f);
        HowToPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);

        SE.PlayOneShot(Slide);
    }

    public void CloseHowTo()
    {
        HowToPanel.transform.DOScale(new Vector3(0f, 0f, 0f), 0.1f);
        Invoke("Close", 0.1f);

        SE.PlayOneShot(Slide);
    }

    public void Close()
    {
        HowToPanel.SetActive(false);
    }

    IEnumerator VolumeDown()
    {
        while (BGM.volume > 0)
        {
            BGM.volume -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
