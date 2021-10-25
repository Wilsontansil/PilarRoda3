using System.Collections;
using System.Collections.Generic;
using Colyseus;
using UnityEngine;

public class MyServerManager : ColyseusManager<MyServerManager>
{
    //protected ColyseusClient client;

    public ColyseusClient Client
    {
        get { return client; }
    }



}
