using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NumberCounter : MonoBehaviour
{
    public TextMeshProUGUI TextJackpot;
    public TextMeshProUGUI TextTotal;
    //TextMeshProUGUI Text;
    public int CountFPS = 10;
    public float Duration = 1f;
    public string NumberFormat = "N0";
    public double _value;

    //public bool isFinishCounter;
    public double Value
    {
        get
        {
            return _value;
        }
        set
        {
            UpdateText(value);
            _value = value;
        }
    }
    private Coroutine CountingCoroutine;

    //private void Awake()
    //{
    //    Text = GetComponent<TextMeshProUGUI>();
    //}

    private void UpdateText(double newValue)
    {
        if (CountingCoroutine != null)
        {
            StopCoroutine(CountingCoroutine);
        }

        CountingCoroutine = StartCoroutine(CountText(newValue));
    }

    private IEnumerator CountText(double newValue)
    {
        //isFinishCounter = false;
        WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
        double previousValue = _value;
        double stepAmount;

        if (newValue - previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((float)(newValue - previousValue) / (CountFPS * Duration)); // newValue = -20, previousValue = 0. CountFPS = 30, and Duration = 1; (-20- 0) / (30*1) // -0.66667 (ceiltoint)-> 0
        }
        else
        {
            stepAmount = Mathf.CeilToInt((float)(newValue - previousValue) / (CountFPS * Duration)); // newValue = 20, previousValue = 0. CountFPS = 30, and Duration = 1; (20- 0) / (30*1) // 0.66667 (floortoint)-> 0
        }

        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                }
                TextJackpot.SetText(previousValue.ToString(NumberFormat));
                TextTotal.SetText(previousValue.ToString(NumberFormat));
                yield return Wait;
                //isFinishCounter = true;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount; // (-20 - 0) / (30 * 1) = -0.66667 -> -1              0 + -1 = -1
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }

                TextJackpot.SetText(previousValue.ToString(NumberFormat));
                TextTotal.SetText(previousValue.ToString(NumberFormat));
                yield return Wait;
                //isFinishCounter = true;
            }
        }
    }
}
