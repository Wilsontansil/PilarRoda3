using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

class betManager
{
    public int bet1;
    public int bet2;
    public int bet3;
}
public class BetSpinManager : MonoBehaviour
{
    List<betManager> betManagers;
    public int betTotal;
    public int addition;
    public int MinBet;
    public int MaxBet;
    [SerializeField]int betStatus;// Betmanager Status
    [SerializeField]int bettingStatus;// Beting Status
    [SerializeField] TMP_InputField txtBet;
    [SerializeField] List<Sprite> btnBetSprite;
    [SerializeField] List<Image> imgBtn;

    BtnScriptTurbo turbo;
    GameManager gm;
    private void Awake()
    {
        turbo = FindObjectOfType<BtnScriptTurbo>();
        gm = FindObjectOfType<GameManager>();
        betManagers = new List<betManager>();
    }

    void SettingUpBetManager()
    {
        betManager b1 = new betManager();//bet Manager stage 1
        b1.bet1 = 100;
        b1.bet2 = 200;
        b1.bet3 = 500;
        betManager b2 = new betManager();//bet Manager stage 2
        b2.bet1 = 1000;
        b2.bet2 = 2000;
        b2.bet3 = 5000;
        betManager b3 = new betManager();//bet Manager stage 3
        b3.bet1 = 10000;
        b3.bet2 = 20000;
        b3.bet3 = 50000;
        betManagers.Add(b1);
        betManagers.Add(b2);
        betManagers.Add(b3);
    }
    void SettingUpButtonBet()
    {
        imgBtn[0].GetComponentInChildren<TextMeshProUGUI>().text = SetK(betManagers[betStatus].bet1);
        imgBtn[1].GetComponentInChildren<TextMeshProUGUI>().text = SetK(betManagers[betStatus].bet2);
        imgBtn[2].GetComponentInChildren<TextMeshProUGUI>().text = SetK(betManagers[betStatus].bet3);
    }
    private void Start()
    {
        betStatus = 0;
        bettingStatus = 0;
        betTotal = MinBet;
        SettingUpBetManager();
        SettingUpButtonBet();
        addition = betManagers[0].bet1;
        txtBet.text = betTotal.ToString();
        BetCoin(bettingStatus);
    }
    public void BetCoin(int btnPos)//bet Button
    {
        switch (btnPos)
        {
            case 0:
                addition = betManagers[betStatus].bet1;
                break;
            case 1:
                addition = betManagers[betStatus].bet2;
                break;
            case 2:
                addition = betManagers[betStatus].bet3;
                break;
            default:
                addition = betManagers[betStatus].bet1;
                break;
        }
        for (int i = 0; i < imgBtn.Count; i++)
        {
            if (i == btnPos)
            {
                imgBtn[i].sprite = btnBetSprite[0];
            }
            else
            {
                imgBtn[i].sprite = btnBetSprite[1];
            }
        }
        bettingStatus = btnPos;
    }

    public void Add() // add Button/plus button
    {
        if (!turbo.isContinuePlay && !gm.spin)
        {
            if (betTotal + addition >= MaxBet)
            {
                betTotal = MaxBet;
            }
            else
            {
                betTotal += addition;
            }

            txtBet.text = SetK(betTotal);
        }

    }
    public void Min() // minus button/min button
    {
        if (!turbo.isContinuePlay && !gm.spin)
        {
            if (betTotal - addition < MinBet) return;
            betTotal -= addition;
            txtBet.text = SetK(betTotal);
        }

    }

    public void SetBetStatus(bool IsUp)// bet Status, set stage
    {
        if (IsUp)
        {
            if (betStatus>=2)
            {
                return;
            }
            else
            {
                betStatus += 1;
            }
        }
        else
        {
            if (betStatus<=0)
            {
                return;
            }
            else
            {
                betStatus -= 1;
            }
        }
        SettingUpButtonBet();
        BetCoin(bettingStatus);
    }

    string SetK(float x)
    {
        string result="";
        if (x.ToString().Length>3)
        {
            result = (x / 1000).ToString()+"K";
        }
        else
        {
            result = x.ToString();
        }
        return result;
    }
}
