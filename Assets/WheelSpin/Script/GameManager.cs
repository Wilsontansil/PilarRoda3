using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class RewardDetail
{
    public int RewardID;
    public string RewardName;
    public int RewardPosition;
    public int RewardPercentage;
    public int IsZonk;
    public int IsJackpot;
    public int IsGrandJackpot;
    public int IsBonus;
}
[System.Serializable]
public class ListRewardDetail
{
    public List<RewardDetail> ListReward = new List<RewardDetail>();
}


[System.Serializable]
public class RewardHistoryUser
{
    public string RewardTime;
    public string RewardName;
}
[System.Serializable]
public class ListHistoryUser
{
    public List<RewardHistoryUser> ListHistoryReward = new List<RewardHistoryUser>();
}

[System.Serializable]
public class RewardJackpotHistoryUser
{
    public string RewardTime;
    public string UserName;
    public string RewardName;
}
[System.Serializable]
public class ListHistoryUserJackpot
{
    public List<RewardJackpotHistoryUser> ListHistoryJackpot = new List<RewardJackpotHistoryUser>();
}
public class GameManager : MonoBehaviour
{
    [Header("Wheel")]
    [SerializeField] String name;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject button;
    public GameObject btnAnim;
    [SerializeField] TextMeshProUGUI txtUserName;
    [SerializeField] TextMeshProUGUI txtUserID;
    [SerializeField] TextMeshProUGUI txtCoin;
    [SerializeField] GameObject bulbOutter;
    [SerializeField] GameObject bulbMid1;
    [SerializeField] GameObject bulbMid2;
    [SerializeField] TMP_InputField txtfield;
    [SerializeField] GameObject prefLamp;
    [SerializeField] GameObject parent;
    [SerializeField] Image indicator;

    [Header("WinLose")]
    [SerializeField] GameObject Message;
    [SerializeField] GameObject disablePanel;
    public GameObject spineJackpot;

    //[Header("Potrait")]
    //[SerializeField] TextMeshProUGUI txtReward;//nullable

    [Header("History User")]
    [SerializeField] List<TextMeshProUGUI> txtHistory;
    [SerializeField] List<GameObject> txtHistoryJackpot;


    [Header("Sound")]
    [SerializeField] AudioClip clipClickBtn;
    [SerializeField] AudioClip clipSpinning;
    [SerializeField] AudioClip clipWin;
    [SerializeField] AudioClip clipJackPot;

    [Header("Animation")]
    public GameObject TextIDR;


    [Header("GameManager")]
    [HideInInspector]public NumberCounter numberCount;
    BetSpinManager betManager;
    [HideInInspector]public UserInfoManager userInfo;
    ListRewardDetail listReward;
    SpinWheelNew spinWheel;
    public bool spin;
    public bool isFinishSendData;
    int repeatAnim;
    public int tempPosWin;
    double total;
    [HideInInspector]public bool animationMiddleBulb;
    BtnScriptTurbo turbo;

    private GameClient client;

    private void Awake()
    {
        userInfo = FindObjectOfType<UserInfoManager>();
        spinWheel = FindObjectOfType<SpinWheelNew>();
        turbo = FindObjectOfType<BtnScriptTurbo>();
        betManager = FindObjectOfType<BetSpinManager>();
        numberCount = FindObjectOfType<NumberCounter>();
        //Application.targetFrameRate = 60;
    }

    void SetUser()
    {
        txtUserID.text = "ID : " + userInfo.UserID;
        txtUserName.text = "User : " + userInfo.UserName;
        //txtCoin.text = string.Format("{0:#,0.##}", userInfo.UserCoin);
        indicator.color = Color.green;
    }
    private void Start()
    {
        try
        {
            clientConnect(false);
        }
        catch (Exception e)
        {
            Debug.Log("Error " + e.Message);
        }
        SetUser();
        numberCount.Value = userInfo.UserCoin;
        REFRESH();
        isFinishSendData = true;
        ReloadReward();
        ReloadUserHistory();
        disablePanel.SetActive(true);//disable user touch until grab all data
        //ClearJackpot();
        InvokeRepeating("BulbRound", 0, .15f);//animation bulb
        if (animationMiddleBulb)//animation bulb
        {
            InvokeRepeating("BulbMiddle", 0, .5f);
        }

    }
    void REFRESH()
    {
        repeatAnim = 0;
        tempPosWin = 0;
        total = 0;
        button.GetComponent<Button>().interactable = true;
    }
    void ClearJackpot()
    {
        for (int i = 0; i < txtHistoryJackpot.Count; i++)
        {
            txtHistoryJackpot[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
            txtHistoryJackpot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-";
            txtHistoryJackpot[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "-";
        }
    }
    public void ReloadJackPotHistory()
    {
        StartCoroutine(GetJackpotDetail());
    }
    public void ReloadReward()
    {
        //client.Send("getallreward", "");
        StartCoroutine(GetRewardDetail());
    }


    public void BulbRound()
    {
        if (repeatAnim>28)
        {
            CancelInvoke("BulbRound");
            bulbOutter.SetActive(false);
            repeatAnim = 0;
            InvokeRepeating("BulbRound2",0,.1f);
        }
        else
        {
            bulbOutter.transform.eulerAngles += new Vector3(0, 0, 18);
            repeatAnim++;
        }

    }
    void BulbRound2()
    {
        if (repeatAnim<10)
        {
            GameObject gb = Instantiate(prefLamp);
            gb.transform.SetParent(parent.transform);
            gb.transform.localScale = Vector3.one;
            gb.transform.localPosition = new Vector3(0, 2);
            gb.transform.eulerAngles = new Vector3(0, 0, repeatAnim * 18);
            repeatAnim++;
        }
        else
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (i % 2 == 0)
                {
                    parent.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            repeatAnim = 0;
            CancelInvoke("BulbRound2");
            InvokeRepeating("BulbRound3",0,.1f);
        }

    }
    void BulbRound3()
    {
        if (repeatAnim<30)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    parent.transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    parent.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            repeatAnim++;
        }
        else
        {
            repeatAnim = 0;
            CancelInvoke("BulbRound3");
            foreach (Transform item in parent.transform)
            {
                Destroy(item.gameObject);
            }
            bulbOutter.SetActive(true);
            InvokeRepeating("BulbRound",0,.15f);
        }

    }
    public void BulbMiddle()
    {
        if (bulbMid1.activeInHierarchy)
        {
            bulbMid1.SetActive(false);
            bulbMid2.SetActive(true);
        }
        else
        {
            bulbMid1.SetActive(true);
            bulbMid2.SetActive(false);
        }
    }

    public void PointerDown()
    {
        if (userInfo.UserID != 0)
        {
            if (userInfo.UserCoin>0)
            {
                if (userInfo.UserCoin >= betManager.betTotal)
                {
                    if (!spin)
                    {
                        Debug.Log("Spin");
                        spin = true;
                        isFinishSendData = false;
                        if (!turbo.IsTurboMode)
                        {
                            LeanAudio.play(clipSpinning);
                        }
                        LeanAudio.play(clipClickBtn);
                        LeanTween.scaleX(arrow, .8f, .2f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => LeanTween.scaleX(arrow, 1f, .2f).setEase(LeanTweenType.easeOutBounce));
                        indicator.color = Color.red;
                        button.GetComponent<Button>().interactable = false;
                        Credit credit = new Credit();
                        credit.coin = betManager.betTotal;
                        userInfo.UserCoin -= betManager.betTotal;
                        numberCount._value = userInfo.UserCoin;
                        //client.Send("bet", credit);   //tester wilson
                        txtCoin.text = string.Format("{0:#,0.##}", userInfo.UserCoin);
                        spinWheel.SpinBTN();
                        if (turbo.IsAutoPlay)
                        {
                            turbo.isContinuePlay = true;
                            turbo.btnCancel.SetActive(true);
                            turbo.txtSpin.text = "Cancel";
                            LeanTween.scale(btnAnim, new Vector3(.85f, .85f), .1f).setEase(LeanTweenType.easeInElastic);
                        }
                        else
                        {
                            LeanTween.scale(btnAnim, new Vector3(.85f, .85f), .1f).setEase(LeanTweenType.easeInElastic).setOnComplete(() => LeanTween.scale(btnAnim, Vector3.one, .1f).setEase(LeanTweenType.easeOutElastic));
                        }

                    }
                }
                else
                {
                    if (!Message.activeInHierarchy)
                    {
                        Message.SetActive(true);
                        Message.GetComponent<WinAnimation>().Message("Jumlah Bet Melebihi koin anda");
                        turbo.StopAllMode();
                    }

                }

            }
            else
            {
                if (!Message.activeInHierarchy)
                {
                    Message.SetActive(true);
                    Message.GetComponent<WinAnimation>().Message("Koin Anda Telah Habis");
                    turbo.StopAllMode();
                }
            }
        }


    }
    void ProcessJsonDataList(string url)
    {
        listReward = JsonUtility.FromJson<ListRewardDetail>(url);

        for (int i = 0; i < spinWheel.txtReward.Count; i++)
        {
            if (i<listReward.ListReward.Count)
            {
                spinWheel.prize[i] = listReward.ListReward[i].RewardName;
                //spinWheel.txtReward[i].text = KiloFormat(float.Parse(listReward.ListReward[i].RewardName));
                spinWheel.txtReward[i].text = listReward.ListReward[i].RewardName;

            }
            else
            {
                spinWheel.prize[i] = "Zonk";
                spinWheel.txtReward[i].text = "Zonk";
            }

        }
        MultipleReward();
        txtfield.onValueChanged.AddListener(delegate { MultipleReward(); });
        disablePanel.SetActive(false);
    }

    public string KiloFormat(float num)
    {
        if (num >= 100000000)
            return (num / 1000000).ToString("#,0M");

        if (num >= 10000000)
            return (num / 1000000).ToString("0.#") + "M";

        if (num >= 100000)
            return (num / 1000).ToString("#,0K");

        if (num >= 10000)
            return (num / 1000).ToString("0.#") + "K";

        return num.ToString("#,0");
    }
    public void MultipleReward()
    {
        for (int i = 0; i < spinWheel.txtReward.Count; i++)
        {
            //spinWheel.txtReward[i].text = KiloFormat(float.Parse(listReward.ListReward[i].RewardName) * betManager.betTotal);
            spinWheel.txtReward[i].text = string.Format("{0:#,0.##}", float.Parse(listReward.ListReward[i].RewardName) * betManager.betTotal);
        }
        if (SceneManager.GetActiveScene().name == "WheelSpin")
        {
            spinWheel.txtJackpot100x.text = string.Format("{0:#,0.##}", float.Parse(listReward.ListReward[6].RewardName) * betManager.betTotal);
            spinWheel.txtJackpot20x.text = string.Format("{0:#,0.##}", float.Parse(listReward.ListReward[16].RewardName) * betManager.betTotal);
        }
    }

    void ProcessJsonJackpotDataList(string url)
    {
        ListHistoryUserJackpot ListHistoryJackpot = JsonUtility.FromJson<ListHistoryUserJackpot>(url);

        for (int i = 0; i < txtHistory.Count; i++)
        {
            if (i < ListHistoryJackpot.ListHistoryJackpot.Count)
            {
                txtHistoryJackpot[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ListHistoryJackpot.ListHistoryJackpot[i].RewardTime.ToString();
                if (ListHistoryJackpot.ListHistoryJackpot[i].UserName.Length > 6)
                {
                    txtHistoryJackpot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ListHistoryJackpot.ListHistoryJackpot[i].UserName.Substring(0, 6) + "...";
                }
                else
                {
                    txtHistoryJackpot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ListHistoryJackpot.ListHistoryJackpot[i].UserName.Substring(0, 6);
                }
                txtHistoryJackpot[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =KiloFormat(float.Parse(ListHistoryJackpot.ListHistoryJackpot[i].RewardName.ToString()));
            }
            else
            {
                txtHistoryJackpot[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
                txtHistoryJackpot[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-";
                txtHistoryJackpot[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "-";
            }

        }

    }

    void ProcessJsonDataListUserHistory(string url)
    {
        ResetListHistoryUser();
        ListHistoryUser ListHistoryUser = JsonUtility.FromJson<ListHistoryUser>(url);
        for (int i = 0; i < txtHistory.Count; i++)
        {
            if (i <ListHistoryUser.ListHistoryReward.Count)
            {
                txtHistory[i].text = ListHistoryUser.ListHistoryReward[i].RewardTime.ToString();
                txtHistory[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = KiloFormat(float.Parse(ListHistoryUser.ListHistoryReward[i].RewardName));
            }
            else
            {
                txtHistory[i].text = "-";
                txtHistory[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
            }


        }
        //if (ListHistoryUser.ListHistoryReward.Count>0)
        //{

        //    if (SceneManager.GetActiveScene().name == "WheelSpinPotrait")
        //    {
        //        txtReward.text = "+ " + KiloFormat(float.Parse(ListHistoryUser.ListHistoryReward[0].RewardName));
        //    }
        //}

    }


    void ResetListHistoryUser()
    {
        for (int i = 0; i < txtHistory.Count; i++)
        {
            txtHistory[i].text = "-";
            txtHistory[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
        }
    }
    //IEnumerator SendHistory(int rewardID,int userID,string total)
    //{
    //    SaveHistory savehistory = new SaveHistory();
    //    savehistory.rewardid = rewardID;
    //    savehistory.userid = userID;
    //    savehistory.total = total;
    //    client.Send("savehistory", savehistory);
    //    ReloadUserHistory();
    //    yield return new WaitForSeconds(1f);

    //    StopCoroutine("SendHistory");
    //    isFinishSendData = true;
    //    spin = false;
    //    indicator.color = Color.green;

    //}

    //IEnumerator GetHistoryUser(int userID)
    //{
    //    String url = "";
    //    if(name == "wheel1")
    //    {
    //        url = config.httpurl + "/api/rewardHistory/wheel1/" + userID;
    //    } else
    //    {
    //        url = config.httpurl + "/api/rewardHistory/wheel2/" + userID;
    //    }
    //    UnityWebRequest www = UnityWebRequest.Get(url);
    //    www.SetRequestHeader("X-Game-Token", client.gametoken());
    //    yield return www.SendWebRequest();
    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        if (www.downloadHandler.text == "0")
    //        {
    //            Debug.Log("Errorrr");
    //        }
    //        else
    //        {
    //            ProcessJsonDataListUserHistory(www.downloadHandler.text);
    //        }

    //    }
    //    StopCoroutine("GetHistoryUser");


    //}

    public void ReloadUserHistory()
    {
        StartCoroutine(GetHistoryUser(userInfo.UserID));
    }

    ////////////////////////Tester Wilson/////////////////////////////////////////

    IEnumerator GetCoinUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userInfo.UserID);

        using (UnityWebRequest www = UnityWebRequest.Post(UserInfoManager.linkWeb + "WheelSpin/GetUserCoin.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                StartCoroutine(GetCoinUser());
            }
            else
            {
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Errorrr");
                    StartCoroutine(GetCoinUser());
                }
                else
                {
                    userInfo.UserCoin = double.Parse(www.downloadHandler.text);
                    //numberCount._value = userInfo.UserCoin;
                    //txtCoin.text = string.Format("{0:#,0.##}", userInfo.UserCoin);
                    StopCoroutine(GetCoinUser());
                }

            }
        }
    }
    IEnumerator SendHistory(int rewardID, int userID)
    {
        Debug.Log("Send History");
        WWWForm form = new WWWForm();
        form.AddField("RewardID", rewardID);
        form.AddField("Multiple", betManager.betTotal);
        form.AddField("Total", total.ToString());
        form.AddField("UserID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post(UserInfoManager.linkWeb + "WheelSpin/InsertHistoryManager.php", form))
        {

            yield return www.SendWebRequest();
            Debug.Log("Send WebRequest");
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error + " Send Data");
                isFinishSendData = false;
                StopAllCoroutines();
                StartCoroutine(SendHistory(listReward.ListReward[tempPosWin].RewardID, userInfo.UserID));
            }
            else
            {
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Error Send Data");
                    isFinishSendData = false;
                    StopAllCoroutines();
                    StartCoroutine(SendHistory(listReward.ListReward[tempPosWin].RewardID, userInfo.UserID));

                }
                else
                {
                    Debug.Log("FinishSendData " + www.downloadHandler.text);
                    yield return StartCoroutine(GetCoinUser());
                    yield return StartCoroutine(GetHistoryUser(userID));
                    isFinishSendData = true;
                    indicator.color = Color.green;
                    spin = false;
                    StopCoroutine("SendHistory");
                }

            }
        }

    }
    IEnumerator GetRewardDetail()
    {
        UnityWebRequest www = UnityWebRequest.Get(UserInfoManager.linkWeb + "WheelSpin/GetRewardDetail.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetRewardDetail());
        }
        else
        {
            if (www.isDone)
            {
                //Debug.Log(www.downloadHandler.text);
                ProcessJsonDataList(www.downloadHandler.text);
                StopCoroutine(GetRewardDetail());
            }
            else
            {
                Debug.LogError("Errror Connection");
                StartCoroutine(GetRewardDetail());

            }
        }

    }

    IEnumerator GetJackpotDetail()
    {
        UnityWebRequest www = UnityWebRequest.Get(UserInfoManager.linkWeb + "WheelSpin/GetJackpotHistory.php");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.LogError("Errror Connection");
            StartCoroutine(GetJackpotDetail());
        }
        else
        {
            if (www.isDone)
            {
                Debug.LogError("Get Jackpot Data");
                Debug.Log(www.downloadHandler.text);
                ProcessJsonJackpotDataList(www.downloadHandler.text);
                StopCoroutine(GetJackpotDetail());
            }
            else
            {
                Debug.LogError("Errror Getdata");
                StartCoroutine(GetJackpotDetail());
            }
        }

    }
    IEnumerator GetHistoryUser(int userID)
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", userID);

        using (UnityWebRequest www = UnityWebRequest.Post(UserInfoManager.linkWeb + "WheelSpin/GetUserRewardHistory.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                StartCoroutine(GetHistoryUser(userID));
            }
            else
            {
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Errorrr");
                    StartCoroutine(GetHistoryUser(userID));
                }
                else
                {
                    //Debug.Log(www.downloadHandler.text);
                    ProcessJsonDataListUserHistory(www.downloadHandler.text);
                    StopCoroutine("GetHistoryUser");
                }

            }
        }


    }
    ////////////////////////////////////////////////////////////////////////////



    //IEnumerator GetJackpotDetail()
    //{
    //    String url = "";
    //    if (name == "wheel1")
    //    {
    //        url = config.httpurl + "/api/jackpot/wheel1";
    //    }
    //    else
    //    {
    //        url = config.httpurl + "/api/jackpot/wheel2";
    //    }
    //    UnityWebRequest www = UnityWebRequest.Get(url);
    //    www.SetRequestHeader("X-Game-Token", client.gametoken());
    //    yield return www.SendWebRequest();
    //    if (www.isNetworkError)
    //    {
    //        Debug.LogError("Errror Connection");
    //    }
    //    else
    //    {
    //        if (www.isDone)
    //        {
    //            //Debug.Log(www.downloadHandler.text);
    //            ProcessJsonJackpotDataList(www.downloadHandler.text);

    //        }
    //    }
    //    StopCoroutine(GetJackpotDetail());

    //}

    public void SpinAgain()
    {
        StartCoroutine(IntervalWin());
    }
    IEnumerator IntervalWin()
    {
        if (turbo.IsAutoPlay)
        {
            yield return new WaitForSeconds(.4f);

        }
        else
        {
            yield return new WaitForSeconds(.5f);
        }
        REFRESH();
        StopAllCoroutines();
        if (turbo.isContinuePlay)
        {
            if (turbo.IsAutoPlay)
            {
                PointerDown();
            }
        }
        else
        {
            StopAllCoroutines();
        }

    }

    public void CheckWinLose(int pos)
    {

        tempPosWin = pos;
        total = double.Parse(listReward.ListReward[pos].RewardName) * betManager.betTotal;
        Message.SetActive(true);
        if (listReward.ListReward[pos].IsZonk == 0)
        {
            Debug.Log("CheckWinLose = "+ pos);
            if (listReward.ListReward[pos].IsJackpot == 1)
            {
                Message.GetComponent<WinAnimation>().WinGameSpine(string.Format("{0:#,0.##}", total), true, false,false);
                //StartCoroutine(SendHistory(listReward.ListReward[pos].RewardID, userInfo.UserID));
                LeanAudio.play(clipJackPot);
                turbo.StopAllMode();
                spineJackpot.SetActive(true);
            }
            else if (listReward.ListReward[pos].IsGrandJackpot == 1)
            {
                Message.GetComponent<WinAnimation>().WinGameSpine(string.Format("{0:#,0.##}", total), false, true,false);
                LeanAudio.play(clipJackPot);
                turbo.StopAllMode();
                spineJackpot.SetActive(true);
            }
            else if (listReward.ListReward[pos].IsBonus== 1)
            {
                Message.GetComponent<WinAnimation>().WinGameSpine(string.Format("{0:#,0.##}", total), false, false, true);
                LeanAudio.play(clipJackPot);
                turbo.StopAllMode();
                spineJackpot.SetActive(true);
            }
            else
            {
                LeanAudio.play(clipWin);
                //Message.SetActive(true);
                Message.GetComponent<WinAnimation>().TextPopUpFly(string.Format("{0:#,0.##}", total));
                //StartCoroutine(SendHistory(listReward.ListReward[pos].RewardID, userInfo.UserID));

            }
            if (Message.activeInHierarchy)
            {
                PopUpTXTIDR();
            }

        }
        else
        {
            //Message.SetActive(true);
            Message.GetComponent<WinAnimation>().TextPopUpFly(string.Format("{0:#,0.##}", total));
            //StartCoroutine(SendHistory(listReward.ListReward[pos].RewardID,userInfo.UserID));
        }
        StartCoroutine(SendHistory(listReward.ListReward[pos].RewardID, userInfo.UserID));
        Debug.Log("CheckWinLose");
        //StartCoroutine(GetJackpotDetail());
    }
    void PopUpTXTIDR()
    {
        LeanTween.scale(TextIDR, new Vector3(1.2f, 1.2f), .1f).setEase(LeanTweenType.easeOutBounce).setLoopPingPong(2);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (turbo.btnCancel.activeInHierarchy)
            {
                turbo.StopAllMode();
            }
            else
            {
                if (isFinishSendData)
                {
                    if (!Message.activeInHierarchy)
                    {

                        PointerDown();
                    }
                }
            }


        }
        //if (isFinishSendData)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        if (!Message.activeInHierarchy)
        //        {
        //            if (turbo.btnCancel.activeInHierarchy)
        //            {
        //                turbo.StopAllMode();
        //            }
        //            else
        //            {
        //                PointerDown();
        //            }

        //        }

        //    }
        //}


    }

    // Game Client Handler

    private void clientConnect(bool reconnect)
    {
        client = GameClient.Instance;

        //Debug.Log(client.connected());
        if (!client.connected())
        {
            //Debug.Log("re connect true");
            //client.Connect();
            client.Join("wheel");

        }
        removeListener();
        client.OnGameRewardChange += onGameRewardChangeHandler;
        client.OnGameError += onGameErrorHandler;
        client.onDisconnect += onDisconnectHandler;
        client.onReconnecting += onReconnectingHandler;
        client.OnGameCoinChange += GameCoinChangeHandler;
    }

    private void GameCoinChangeHandler(object sender, long value)
    {
        //Debug.Log("coin = " + value);
        txtCoin.text = string.Format("{0:#,0.##}", userInfo.UserCoin);
    }

    private void onGameRewardChangeHandler(object sender, string reward)
    {
        ProcessJsonDataList(reward);
    }

    private void onGameErrorHandler(object sender, string message)
    {
        turbo.StopAllMode();
        StopAllCoroutines();
    }

    private void onDisconnectHandler(object sender, int code)
    {
        //Debug.Log("cek code disconnect. apa perlu reconnect?" + code);
        if (code > 1001)
        {
            if (client.isconnect)
            {
                client.isconnect = false;
                clientConnect(true);
            }
        }
    }

    private void onReconnectingHandler(object sender, string message)
    {
        Debug.Log("Tampilkan screen reconnecting");
        // TODO: Buat Screen Reconnecting
    }

    private void removeListener()
    {
        client.OnGameRewardChange -= onGameRewardChangeHandler;
        client.OnGameError -= onGameErrorHandler;
        client.onDisconnect -= onDisconnectHandler;
        client.onReconnecting -= onReconnectingHandler;
    }

}
