using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NumberCounterUpdater : MonoBehaviour
{
    public NumberCounter NumberCounter;
    //public TMP_InputField InputField;

    public void SetValue(string x)
    {
        double value;

        if (double.TryParse(x, out value))
        {
            NumberCounter.Value = value;
        }
    }
}
