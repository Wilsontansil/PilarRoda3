//
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
//
// GENERATED USING @colyseus/schema 1.0.25
//

using Colyseus.Schema;

[UnityEngine.Scripting.Preserve]
public partial class WheelState : Schema
{
    [Type(0, "string")]
    public string roomId = default(string);

    [Type(1, "string")]
    public string sessionId = default(string);

    [Type(2, "string")]
    public string phase = default(string);

    [Type(3, "int64")]
    public long coin = default(long);

    [Type(4, "uint64")]
    public ulong id = default(ulong);

    [Type(5, "string")]
    public string email = default(string);

    [Type(6, "string")]
    public string username = default(string);

    [Type(7, "string")]
    public string level = default(string);

    [Type(8, "string")]
    public string gametoken = default(string);

    [Type(9, "string")]
    public string rewards = default(string);

    [Type(10, "int64")]
    public long bet = default(long);
}

