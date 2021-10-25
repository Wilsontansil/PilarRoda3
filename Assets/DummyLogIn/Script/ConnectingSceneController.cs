//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using System;
//using System.Runtime.InteropServices;

//public class ConnectingSceneController : MonoBehaviour
//{
//    [DllImport("__Internal")]
//    private static extern string GetURLFromPage();

//    public Text message;

//    private GameClient client;
//    private string url;


//    void Start()
//    {

//        url = @"http://localhost:8000/games/wheel1/?OggiMmlzgUcAImdsSM5sHWvfJulxn4LFwkxjOiohYq0W7dnBrY2DkPr9frZfNrSKpwGP32opuF9rEV2ldwfdRWLOXYjOKLYZcDyKM9lOoctKiYVYJr7b4wW6nvNaDYxM";

//        //url = GetURLFromPage();
//        message.text = "Preparing...";


//        try
//        {
//            client = GameClient.Instance;
//            client.token = getTokenFromURL(url);
//            client.Connect();
//            client.Join("wheel");
//            client.OnGamePhaseChange += GamePhaseChangeHandler;
//        }
//        catch (Exception e)
//        {
//            Debug.Log("Error " + e.Message);
//        }

//    }

//    private string getTokenFromURL(string url)
//    {
//        string[] urlList = url.Split('?');
//        if (urlList.Length < 2) return "";

//        return urlList[1];
//    }

//    private void GamePhaseChangeHandler(object sender, string phase)
//    {
//        if (phase == "ready")
//        {
//            Debug.Log("Phase changed to ready");
//            SceneManager.LoadScene("WheelSpin");
//        } else if (phase == "login")
//        {
//            client.login();
//        }
//    }
//}
