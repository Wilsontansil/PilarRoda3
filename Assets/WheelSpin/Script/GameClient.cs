using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Colyseus;
using Colyseus.Schema;
using System;

//public class PowerUpMessage
//{
//    string kind;
//}
[UnityEngine.Scripting.Preserve]
public class GameClient : GenericSingleton<GameClient>
{
    public EventHandler<WheelState> OnInitialState;
    public EventHandler<string> OnGamePhaseChange;
    public EventHandler<long> OnGameCoinChange;
    public EventHandler<ulong> OnGameIDChange;
    public EventHandler<string> OnGameEmailChange;
    public EventHandler<string> OnGameUsernameChange;
    public EventHandler<string> OnGameLevelChange;
    public EventHandler<string> OnGameGameTokenChange;
    public EventHandler<string> OnGameSessionIdChange;
    public EventHandler<string> OnGameRewardChange;
    public EventHandler<string> OnGameError;
    public EventHandler<int> onDisconnect;
    public EventHandler<string> onReconnecting;
    public EventHandler<int> onSpinResult;

    public string token;

    protected ColyseusClient client;
    private bool initialStateReceived = false;

    ColyseusRoom<WheelState> myroom;
    private string roomId;
    private string sessionId;
    private int pingcount = 0;
    public bool isconnect = false;

    public void Connect()
    {
        MyServerManager.Instance.InitializeClient();
        client = MyServerManager.Instance.Client;
        //Debug.Log(client);

    }

    public string gametoken()
    {
        return myroom.State.gametoken;
    }

    public bool connected()
    {
        return myroom.colyseusConnection.IsOpen;

        //return client != null;
    }

    async public void Join(string gameroom)
    {
        try
        {
            myroom = await client.JoinOrCreate<WheelState>(gameroom);
        }
        catch (Exception ex)
        {
            //Debug.Log("Error after join or create");
            isconnect = true;
            // Error, tampilkan pesan error connect atau munculkan pesan reconnecting....
            onReconnecting?.Invoke(this, "reconnecting");
            return;
        }

        isconnect = true;
        StopAllCoroutines();
        //StartCoroutine(ping());

        myroom.OnStateChange += (state, isFirstState) =>
        {
            if (isFirstState && !initialStateReceived)
            {
                //Debug.Log("this is the first room state!");
                initialStateReceived = true;
                OnInitialState?.Invoke(this, state);
            }

            //Debug.Log("the room state has been updated");
        };

        myroom.OnJoin += () =>
        {
            //Debug.Log("Join");
        };

        myroom.OnLeave += (code) =>
        {
            //Debug.Log("client left the room with code " + code);
            if (code <= 1001)
            {
                isconnect = false;
            }
            onDisconnect?.Invoke(this, (int)code);


        };

        myroom.OnMessage<string>("error", (message) =>
        {
            //Debug.Log("Error socket " + message);
            OnGameError?.Invoke(this, (string)message);
        });

        myroom.OnMessage<string>("pong", (message) =>
        {
            //Debug.Log("receive pong ");
            pingcount = 0;
        });

        myroom.OnMessage<int>("spinresult", (message) =>
        {
            Debug.Log("result " + message);
            onSpinResult?.Invoke(this, message);
        });



        //myroom.OnMessage<PowerUpMessage>("powerup", (message) => {
        //    Debug.Log("message received from server");
        //    Debug.Log(message);
        //});

        myroom.State.OnChange += (changes) =>
        {
            changes.ForEach((change) =>
            {
                if (change.Field == "phase")
                {
                    //Debug.Log("phase changed");
                    OnGamePhaseChange?.Invoke(this, (string)change.Value);
                }
                else if (change.Field == "coin")
                {
                    //Debug.Log("coin changed");
                    OnGameCoinChange?.Invoke(this, (long)change.Value);
                }
                else if (change.Field == "id")
                {
                    //Debug.Log("coin changed");
                    OnGameIDChange?.Invoke(this, (ulong)change.Value);
                }
                else if (change.Field == "email")
                {
                    //Debug.Log("email changed");
                    OnGameEmailChange?.Invoke(this, (string)change.Value);
                }
                else if (change.Field == "username")
                {
                    //Debug.Log("username changed");
                    OnGameUsernameChange?.Invoke(this, (string)change.Value);
                }
                else if (change.Field == "level")
                {
                    //Debug.Log("level changed");
                    OnGameLevelChange?.Invoke(this, (string)change.Value);
                }
                else if (change.Field == "gametoken")
                {
                    //Debug.Log("game token changed");
                    OnGameGameTokenChange?.Invoke(this, (string)change.Value);
                }
                else if (change.Field == "sessionId")
                {
                    //Debug.Log("session id changed");
                    OnGameSessionIdChange?.Invoke(this, (string)change.Value);
                }
                else if (change.Field == "rewards")
                {
                    //Debug.Log("rewards changed");
                    OnGameRewardChange?.Invoke(this, (string)change.Value);
                }
                else if (change.Field == "roomId")
                {
                    roomId = (string)change.Value;
                }
                else if (change.Field == "sessionId")
                {
                    sessionId = (string)change.Value;
                }


            });
        };

    }

    public void login()
    {
        Debug.Log(token);
        Login login = new Login();
        login.gametoken = token;
        myroom.Send("login", login);

    }

    public void Send(string command, object args)
    {
        myroom.Send(command, args);
    }

    IEnumerator ping()
    {
        if (pingcount == 2)
        {
            pingcount = 0;
            onDisconnect?.Invoke(this, 2000);

        }
        try
        {
            //Debug.Log("send ping " + pingcount);
            myroom.Send("ping", "");
            pingcount += 1;
        }
        catch (Exception ex)
        {
            Debug.Log("failed ping to server " + ex);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(ping());
    }


}