using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using NCMB;

public class GameManager : MonoBehaviour
{
    [SerializeField] StageManager stageManager;

    int score;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] float feverGauge = 0;
    [SerializeField] float feverLimit;

    [SerializeField] float timer = 60.0f;
    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] float feverTimer = 7.5f;
    bool isFever = false;

    bool isPlay = false;

    [SerializeField] Image FadePanel;

    [SerializeField] GameObject ThiefObj;
    [SerializeField] SpriteRenderer Thief;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite angerSprite;
    [SerializeField] Sprite gradSprite;

    [SerializeField] SoundManager soundManager;

    [SerializeField] Image FrastGauge;

    [SerializeField] Image FinishCard;

    [SerializeField] TextMeshProUGUI resultScoreText;
    [SerializeField] TextMeshProUGUI resultJuwelText;
    int feverCount = 0;
    [SerializeField] TextMeshProUGUI resultTimeText;
    int solveCount = 0;
    [SerializeField] TextMeshProUGUI resultSolveText;
    int breakCount = 0;
    [SerializeField] TextMeshProUGUI resultBreakText;
    [SerializeField] TextMeshProUGUI resultTotalText;

    [SerializeField] GameObject resultRank;
    [SerializeField] TextMeshProUGUI resultRankText;

    [SerializeField] GameObject resultButtons;

    int bonusCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        stageManager.LoadStageFromText();
        stageManager.CreateStage();
        stageManager.MakeStage(false);
        stageManager.stageClear += Cleard;
        stageManager.bonusGet += Bonused;

        Invoke("SpotLightPlay", 0.5f);
        Invoke("ReadyPlay", 1.0f);
        Invoke("startPlay", 2.0f);

        FadePanel.color = new Color(0f, 0f, 0f, 1.0f);
    }

    public void SpotLightPlay()
    {
        soundManager.SpotLightPlay();
        DOTween.ToAlpha(
            () => FadePanel.color,
            color => FadePanel.color = color,
            0f, 1f);
    }

    public void ReadyPlay()
    {
        soundManager.ReadyPlay();
    }

    void Update()
    {
        if (isPlay == true)
        { 
            timer -= Time.deltaTime;
            timeText.text = timer.ToString("f1");

            if (timer <= 0)
            {
                timeText.text = "FINISH";
                endPlay();
            }

            if (isFever == true)
            {
                feverTimer -= Time.deltaTime;
                FrastGauge.fillAmount = feverTimer / 7.5f;
            }

            if (feverTimer <= 0)
            {
                endFever();
            }
        }
    }

    public void Bonused()
    {
        score += 3000000;
        scoreText.text = "GOLD:" + score.ToString("N0");
        bonusCount++;

        ThiefObj.transform.DOComplete();

        ThiefObj.transform.DOScaleY(52f, 0.2f).From();

        CancelInvoke();
        Invoke("ChangeGrad", 1.0f);
        Thief.sprite = gradSprite;
    }

    public void Cleard()
    {

        if (isFever == true)
        {
            stageManager.ClearStage(0.05f);
            stageManager.DestroyStage(0.2f);
            score += 300000;
            breakCount++;
        }
        else if (isFever == false)
        {
            stageManager.ClearStage(0.2f);
            stageManager.DestroyStage(0.2f);
            score += 1000000;
            solveCount++;
        }
        
        upFeverGauge();

        stageManager.LoadStageFromText();
        stageManager.CreateStage();
        stageManager.MakeStage(true);


        scoreText.text = "GOLD:" + score.ToString("N0");

        ThiefObj.transform.DOComplete();

        ThiefObj.transform.DOScaleY(52f, 0.2f).From();
    }


    public void upFeverGauge()
    {
        if (isFever == false)
        {
            FrastGauge.color = new Color(1f, 1f, 1f, 200/255f);
            feverGauge++;
            FrastGauge.fillAmount = feverGauge / feverLimit;

            CancelInvoke();
            Invoke("ChangeGrad", 1.0f);
            Thief.sprite = gradSprite;

            if (feverGauge >= feverLimit)
            { 
                startFever();
                FrastGauge.color = new Color(1f,1f,1f,1f);
            }
        }
    }

    public void ChangeGrad()
    {
        if (isFever == false) Thief.sprite = normalSprite;
    }    

    public void startFever()
    {
        stageManager.startFever();
        isFever = true;

        timer += 5.0f;

        Thief.sprite = angerSprite;
        soundManager.AngerPlay();

        feverCount++;
    }

    public void endFever()
    {
        isFever = false;
        feverGauge = 0;

        feverTimer = 7.5f;

        stageManager.endFever();

        Thief.sprite = normalSprite;

        soundManager.AngerEndPlay();
    }

    public void startPlay()
    {
        isPlay = true;
        FadePanel.gameObject.SetActive(false);

        soundManager.bgmPlay();
    }

    public void endPlay()
    {
        stageManager.endFever();
        timer = 0;
        isFever = false;
        isPlay = false;
        FadePanel.gameObject.SetActive(true);

        soundManager.endPlay();
        StartCoroutine("Result");
    }

    IEnumerator Result()
    {
        resultScoreText.text = "";
        resultJuwelText.text = "";
        resultTimeText.text = "";
        resultSolveText.text = "";
        resultBreakText.text = "";
        resultTotalText.text = "";
        resultRankText.text = "";
        resultRank.SetActive(false);
        resultButtons.SetActive(false);

        FinishCard.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f);
        FinishCard.transform.DORotate(new Vector3(0, 0, 0), 0.5f);

        yield return new WaitForSeconds(1.5f);
        yield return null;

        resultScoreText.text = "GOLD:" + score.ToString("N0");
        soundManager.SlidePlay();
        //resultJuwelText.text = "JUWEL:" + bonusCount;

        yield return new WaitForSeconds(0.5f);
        yield return null;

        int totalTime = 60 + (feverCount * 5);
        int minutes = 0;

        while (true)
        {
            minutes++;
            totalTime -= 60;

            if (totalTime <= 60) break;
        }


        resultTimeText.text = "TIME " + minutes.ToString("D2") + ":" + totalTime.ToString("D2");
        soundManager.SlidePlay();

        yield return new WaitForSeconds(0.5f);
        yield return null;

        int total = solveCount + breakCount;

        resultSolveText.text = "SOLVE:" + total;
        soundManager.SlidePlay();

        yield return new WaitForSeconds(1.0f);
        yield return null;

        soundManager.ClearPlay();
        resultRank.SetActive(true);

        if (score >= 100000000)
        { 
            resultRankText.text = "SS";
        }
        else if (score >= 75000000)
        {
            resultRankText.text = "S";
        }
        else if (score >= 50000000)
        {
            resultRankText.text = "A";
        }
        else if (score >= 10000000)
        {
            resultRankText.text = "B";
        }
        else
        {
            resultRankText.text = "C";
        }

        yield return new WaitForSeconds(0.5f);
        yield return null;

        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);

        yield return new WaitForSeconds(0.5f);
        yield return null;

        resultButtons.SetActive(true);
        FadePanel.gameObject.SetActive(false);
    }

    public void OnResetButton()
    {
        stageManager.DestroyStage(0.0f);
        stageManager.CreateStage();
        stageManager.MakeStage(true);
    }

    public void Rank()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
    }

    [TextArea]
    [SerializeField] string[] TweetText;

    public void Tweet()
    {
        int TweetR = Random.Range(0, TweetText.Length);
        int total = solveCount + breakCount;
        naichilab.UnityRoomTweet.Tweet("Frustrated", TweetText[TweetR] + " 今日は鍵を解いたり破壊して、" + total + "個の金庫から" + score.ToString("N0") + "GOLD頂戴した。さらばだー！", "unityroom", "unity1week", "Frusrated_Thief");
    }

    public void Retry()
    {
        soundManager.SlidePlay();
        FadePanel.gameObject.SetActive(true);

        DOTween.ToAlpha(
        () => FadePanel.color,
        color => FadePanel.color = color,
        1f, 1f);

        Invoke("LoadRetry", 1.0f);
    }

    public void LoadRetry()
    {
        SceneManager.LoadScene("Main");
    }

    public void Title()
    {
        soundManager.SlidePlay();
        FadePanel.gameObject.SetActive(true);

        DOTween.ToAlpha(
        () => FadePanel.color,
        color => FadePanel.color = color,
        1f, 1f);

        Invoke("LoadTitle", 1.0f);
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Home");
    }
}
