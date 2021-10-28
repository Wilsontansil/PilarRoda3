using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class ClassUser
{
    public int UserID;
    public string UserName;
    public string UserPhone;
    public int UserCoin;
    public string DateGenerate;

}
public class UserInfoManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string GetURLFromPage();


    public int UserID;
    public string UserName;
    public string UserPhone;
    public double UserCoin;
    public string DateGenerate;

    public static string linkWeb;

    public Text message;

    private GameClient client;
    private string url;

    int count = 0;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        linkWeb = "https://free258789.000webhostapp.com/";
    }
    private void Start()
    {
        //url = @"http://localhost:8000/games/wheel1/?h6BEOZ4t01yO1pr4vp1d3rLu7mZ4JMcxzJYxtLzPoVSNqPwgGq7hAEktLI476UVPCWJB26vL7KdWxmw1Q9I2KVrkekP4y5H7OJNiPsBKohDHEcFvXacGztLOGhaDy5bu";

        ////url = GetURLFromPage();
        //message.text = "Preparing...";

        //try
        //{
        //    client = GameClient.Instance;
        //    client.token = getTokenFromURL(url);
        //    client.Connect();
        //    client.Join("wheel");
        //    client.OnGamePhaseChange += GamePhaseChangeHandler;
        //    client.OnGameCoinChange += GameCoinChangeHandler;
        //    client.OnGameIDChange += GameIDChangeHandler;
        //    client.OnGameEmailChange += GameEmailChangeHandler;
        //    client.OnGameUsernameChange += GameUsernameChangeHandler;
        //    client.OnGameLevelChange += GameLevelChangeHandler;
        //    client.OnGameGameTokenChange += GameGameTokenChangeHandler;
        //    client.OnGameSessionIdChange += GameSessionIdChangeHandler;
        //}
        //catch (Exception e)
        //{
        //    Debug.Log("Error " + e.Message);
        //}
        StartCoroutine(Redeem());
        //InvokeRepeating(nameof(Loading), 0, 1);
    }

    void Loading()
    {
        if (message!=null)
        {
            if (count > 3)
            {
                message.text = "Connecting";
                count = 0;
            }
            else
            {
                message.text += ".";
            }

            count++;
        }

    }
    private string getTokenFromURL(string url)
    {
        string[] urlList = url.Split('?');
        if (urlList.Length < 2) return "";

        return urlList[1];
    }

    private void GamePhaseChangeHandler(object sender, string phase)
    {
        if (phase == "ready")
        {
            //Debug.Log("Phase changed to ready");
            loadScene();
            // Landscape
            //SceneManager.LoadScene("WheelSpin");
            // Potrait
            //SceneManager.LoadScene("WheelSpinPotrait");
        }
        else if (phase == "login")
        {
            client.login();
        }
    }

    private void GameCoinChangeHandler(object sender, long value)
    {
        //Debug.Log("coin = " + value);
        UserCoin = value;
    }

    private void GameEmailChangeHandler(object sender, string value)
    {
        //Debug.Log("email = " + value);
    }

    private void GameUsernameChangeHandler(object sender, string value)
    {
        //Debug.Log("uesrname = " + value);
        UserName = value;
    }

    private void GameLevelChangeHandler(object sender, string value)
    {
        //Debug.Log("level = " + value);
    }

    private void GameGameTokenChangeHandler(object sender, string value)
    {
        //Debug.Log("token = " + value);
    }

    private void GameSessionIdChangeHandler(object sender, string value)
    {
        //Debug.Log("session id = " + value);
    }

    private void GameIDChangeHandler(object sender, int value)
    {
        //Debug.Log("user id = " + value);
        UserID = value;
    }

    private void loadScene()
    {
        Debug.Log("Load scene");
        SceneManager.LoadScene("WheelSpin");
        //SceneManager.LoadScene("WheelSpinPotrait");
        //SceneManager.LoadScene("WheelSpin2");
        //SceneManager.LoadScene("WheelSpin2Potrait");

    }

    void ProcessJsonData(string url)
    {
        ClassUser user = JsonUtility.FromJson<ClassUser>(url);
        UserName = user.UserName;
        UserPhone = user.UserPhone;
        UserCoin = user.UserCoin;
        DateGenerate = user.DateGenerate;
        UserID = user.UserID;



    }

    IEnumerator Redeem()
    {
        WWWForm form = new WWWForm();
        form.AddField("UserID", 17020001);

        using (UnityWebRequest www = UnityWebRequest.Post(linkWeb+"WheelSpin/LogInUser.php", form))
        {

            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                StartCoroutine(Redeem());
            }
            else
            {
                if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Errorrr");
                    StartCoroutine(Redeem());
                }
                else
                {
                    ProcessJsonData(www.downloadHandler.text);
                    StopCoroutine(Redeem());
                    SceneManager.LoadScene("WheelSpin");
                    Debug.Log("Finish Grab Data");


                }

            }
        }

    }
}



