using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource SE;

    [SerializeField] AudioClip SpotLight;
    [SerializeField] AudioClip Ready;
    [SerializeField] AudioClip Slide;
    [SerializeField] AudioClip Clear;
    [SerializeField] AudioClip Break;
    [SerializeField] AudioClip Anger;
    [SerializeField] AudioClip AngerEnd;
    [SerializeField] AudioClip Bonus;
    [SerializeField] AudioClip Juwel;

    public void bgmPlay()
    {
        BGM.Play();
    }

    public void SpotLightPlay()
    {
        SE.PlayOneShot(SpotLight);
    }

    public void ReadyPlay()
    {
        SE.PlayOneShot(Ready);
    }

    public void SlidePlay()
    {
        SE.PlayOneShot(Slide);
    }

    public void ClearPlay()
    {
        SE.PlayOneShot(Clear);
    }

    public void BreakPlay()
    {
        SE.PlayOneShot(Break);
    }

    public void AngerPlay()
    {
        SE.PlayOneShot(Anger);
    }

    public void AngerEndPlay()
    {
        SE.PlayOneShot(AngerEnd);
    }

    public void BonusPlay()
    {
        SE.PlayOneShot(Bonus);
    }

    public void JuwelPlay()
    {
        SE.PlayOneShot(Juwel);
    }

    public void endPlay()
    {
        StartCoroutine("VolumeDown");
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
