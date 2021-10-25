using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField] GameObject Button;
    public void PressButton()
    {
        LeanTween.scale(Button, new Vector3(.85f, .85f), .1f).setEase(LeanTweenType.easeInCubic).setOnComplete(()=> LeanTween.scale(Button, Vector3.one, .2f).setEase(LeanTweenType.easeOutBounce));
    }
}
