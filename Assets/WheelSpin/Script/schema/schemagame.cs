using Colyseus.Schema;

public class Login : Schema
{
    [Type(0, "string")]
    public string gametoken = default(string);
}

public class Credit : Schema
{
    [Type(0, "uint32")]
    public int coin = default(int);
}

public class Debit : Schema
{
    [Type(0, "uint32")]
    public int coin = default(int);
}

public class SaveHistory : Schema

{
    [Type(0, "uint32")]
    public int rewardid = default(int);

    [Type(1, "uint64")]
    public int userid = default(int);

    [Type(2, "string")]
    public string total = default(string);
}