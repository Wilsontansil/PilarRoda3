using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class WinAnimation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtReward;
    [SerializeField] TextMeshProUGUI txtRewardTotal;
    [SerializeField] GameObject gbDark;
    //[SerializeField] List<GameObject> bulb;

    [Header("RewardPopUp")]
    [SerializeField] GameObject textPopUp;
    [SerializeField] GameObject parentPopUp;

    [Header("Message")]
    [SerializeField] GameObject textMessage;
    [SerializeField] GameObject parentMesssage;
    GameManager gm;
    BtnScriptTurbo turbo;
    [SerializeField] bool canClose;

    [Header("Spine Asset")]
    GameObject WinSpine;
    GameObject rewardBar;
    [SerializeField] List<SkeletonDataAsset> animSpine;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        turbo = FindObjectOfType<BtnScriptTurbo>();
    }
    private void OnEnable()
    {
        Refresh();
    }
    private void OnDisable()
    {

        Refresh();
    }
    void Refresh()
    {
        canClose = false;
        txtReward.gameObject.transform.localPosition = new Vector3(0, 17);
        txtRewardTotal.gameObject.transform.localScale = Vector3.zero;
        txtReward.gameObject.transform.localScale = Vector2.one;
        txtReward.GetComponent<TextMeshProUGUI>().alpha = 1;

    }

    public void WinGameSpine(string reward,bool isJackpot,bool isGrandJackpot,bool isBonus)
    {
        gbDark.SetActive(true);
        WinSpine = new GameObject();
        WinSpine.AddComponent<SkeletonAnimation>();
        WinSpine.GetComponent<MeshRenderer>().sortingOrder = 2;
        if(SceneManager.GetActiveScene().name == "WheelSpin") WinSpine.transform.localScale = new Vector3(.4f, .4f); else WinSpine.transform.localScale = new Vector3(.15f, .15f);
        WinSpine.transform.position = new Vector3(0, 2);
        SkeletonAnimation gbSpine = WinSpine.GetComponent<SkeletonAnimation>();
        WinSpine.gameObject.layer = 2;
        if (isBonus)
        {
            gbSpine.skeletonDataAsset = animSpine[0];
        }
        if (isJackpot)
        {
            gbSpine.skeletonDataAsset = animSpine[1];
        }
        if (isGrandJackpot)
        {
            gbSpine.skeletonDataAsset = animSpine[2];
        }
        gbSpine.AnimationName = "animation";

        rewardBar = new GameObject();
        rewardBar.AddComponent<SkeletonAnimation>();
        rewardBar.GetComponent<MeshRenderer>().sortingOrder = 1;
        if (SceneManager.GetActiveScene().name == "WheelSpin")
        {
            rewardBar.transform.localScale = new Vector3(.4f, .4f);
            rewardBar.transform.position = new Vector3(0, -.5f);
        }
        else
        {
            rewardBar.transform.localScale = new Vector3(.25f, .25f);
            rewardBar.transform.position = Vector3.zero;
        }
        SkeletonAnimation rewardSpine = rewardBar.GetComponent<SkeletonAnimation>();
        rewardSpine.skeletonDataAsset = animSpine[3];
        rewardSpine.AnimationName = "start";
        rewardSpine.AnimationState.Complete += delegate
        {
            if (rewardSpine.AnimationName == "start")
            {
                txtReward.gameObject.SetActive(true);
                txtRewardTotal.text = string.Format("{0:#,0.##}", gm.numberCount._value);
                txtReward.text = reward;
                LeanTween.scale(txtRewardTotal.gameObject, Vector2.one, .5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(() => StartCoroutine(CloseWinLose(true)));

            }
            rewardSpine.AnimationName = "loop";
            rewardSpine.loop = true;
            //Debug.Log("Tesdt.........");

        };

    }
    //public void WinGame(string reward, bool isJackpot,string message)
    //{
    //    txtRewardTotal.text = string.Format("{0:#,0.##}", gm.numberCount._value);
    //    txtDesc.gameObject.SetActive(true);
    //    txtReward.gameObject.SetActive(true);
    //    Win.gameObject.SetActive(true);
    //    txtReward.text = reward;
    //    txtDesc.text = message;
    //    LeanTween.scale(Win, Vector3.one, .1f).setEase(LeanTweenType.easeOutBounce);
    //    LeanTween.moveLocalY(txtDesc.gameObject, 44, .3f).setEase(LeanTweenType.easeOutExpo).setOnComplete(() => LeanTween.scale(txtRewardTotal.gameObject, Vector2.one, .5f).setEase(LeanTweenType.easeOutExpo));

    //    ZigZag();
    //    if (isJackpot)
    //    {
    //        btnJackpot.SetActive(true);
    //        btnJackpot.transform.localScale = Vector3.one;
    //    }
    //    else
    //    {
    //        btnJackpot.SetActive(false);
    //        btnJackpot.transform.localScale = Vector3.zero;
    //    }
    //    StartCoroutine(CloseWinLose(isJackpot));
    //}

    void TxtRewardAnim()
    {
        TextMeshProUGUI text = txtReward.GetComponent<TextMeshProUGUI>();
        var color = text.color;
        color.a = 0;
        var fadeoutcolor = color;
        fadeoutcolor.a = 1;
        LeanTween.value(txtReward.gameObject, updateValueExampleCallback, fadeoutcolor, color, .5f).setDelay(.6f).setOnComplete(SetUpdateText);
    }
    void SetUpdateText()
    {
        txtReward.transform.localScale = Vector3.zero;
        txtReward.gameObject.SetActive(false);
        gm.numberCount.Value = gm.userInfo.UserCoin;
        canClose = true;
        gm.SpinAgain();
    }
    void updateValueExampleCallback(Color val)
    {
        txtReward.GetComponent<TextMeshProUGUI>().color = val;
    }
    public void TextPopUpFly(string reward)
    {
        GameObject gb = Instantiate(textPopUp);
        gb.transform.SetParent(parentPopUp.transform);
        gb.transform.localScale = Vector3.one;
        gb.transform.localPosition = new Vector3(70, 10);
        gb.GetComponent<TextMeshProUGUI>().text = "+ "+reward;
        StartCoroutine(CloseWinLose(false));
    }
    public void Message(string x)
    {
        GameObject gb = Instantiate(textMessage);
        gb.transform.SetParent(parentMesssage.transform);
        gb.transform.localScale = Vector3.one;
        gb.transform.localPosition = Vector3.zero;
        gb.GetComponent<TextMeshProUGUI>().text = x;
        LeanTween.scale(gb, new Vector3(1.3f, 1.3f), .5f).setEase(LeanTweenType.easeOutBack).setOnComplete(()=> StartCoroutine(FadeOut(gb)));
    }
    IEnumerator FadeOut(GameObject gb)
    {
        yield return new WaitForSeconds(.2f);
        gb.GetComponent<TextMeshProUGUI>().CrossFadeColor(new Color32(255, 255, 255, 0), 1, true, true);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        StopCoroutine(FadeOut(gb));
        Destroy(gb);
    }

    IEnumerator CloseWinLose(bool isjackpot)
    {
        yield return new WaitUntil(() => gm.isFinishSendData);
        yield return new WaitForSeconds(1);

        StopCoroutine("CloseWinLose");
        if (!isjackpot)
        {
            SetUpdateText();
            gameObject.SetActive(false);

        }
        else
        {
            LeanTween.moveLocalY(txtReward.gameObject, txtRewardTotal.transform.localPosition.y, .3f).setDelay(.2f).setEase(LeanTweenType.easeOutExpo).setOnComplete(() => LeanTween.scale(txtReward.gameObject, new Vector3(1.2f, 1.2f), .4f).setEase(LeanTweenType.easeOutBounce));
            ;
            //txtReward.CrossFadeAlpha(0, 1, true);
            TxtRewardAnim();
        }

    }
    void CloseMessageWn()
    {
        if (canClose)
        {
            Refresh();
            gm.spineJackpot.SetActive(false);
            Destroy(WinSpine);
            Destroy(rewardBar);
            gbDark.SetActive(false);
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (canClose)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CloseMessageWn();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    CloseMessageWn();
                }
            }

        }

    }
    float waitGameOver()
    {
         float x = 0;
        if (turbo.IsAutoPlay)
        {
            x = .5f;
        }
        else
        {
            x = 1;
        }
        return x;
    }
    //int zigZag = 0;
    //void ZigZag()
    //{
    //    zigZag = 0;

    //    InvokeRepeating("Zig", 0, .1f);
    //}
    //void Zig()
    //{
    //    if (zigZag<15)
    //    {
    //        for (int i = 0; i < bulb.Count; i++)
    //        {
    //            if (zigZag%2==0)
    //            {
    //                if (i%2==0)
    //                {
    //                    bulb[i].SetActive(true);
    //                }
    //                else
    //                {
    //                    bulb[i].SetActive(false);
    //                }
    //            }
    //            else
    //            {
    //                if (i % 2 != 0)
    //                {
    //                    bulb[i].SetActive(true);
    //                }
    //                else
    //                {
    //                    bulb[i].SetActive(false);
    //                }
    //            }

    //        }
    //        zigZag += 1;
    //    }
    //    else
    //    {
    //        CancelInvoke("Zig");
    //        zigZag = 0;
    //        InvokeRepeating("IncrementAnim", 0, .1f);
    //    }

    //}

    //void IncrementAnim()
    //{
    //    if (zigZag>bulb.Count)
    //    {
    //        CancelInvoke("IncrementAnim");
    //        //InvokeRepeating("ZigZag", 0, 0);
    //        ZigZag();
    //    }
    //    for (int i = 0; i < bulb.Count; i++)
    //    {
    //        if (i == zigZag)
    //        {
    //            bulb[i].SetActive(true);
    //        }
    //        else
    //        {
    //            bulb[i].SetActive(false);
    //        }
    //    }
    //    zigZag += 1;


    //}
}
