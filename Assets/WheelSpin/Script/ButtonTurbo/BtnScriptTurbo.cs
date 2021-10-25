using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
public class BtnScriptTurbo : MonoBehaviour
{
    public bool IsAutoPlay;
    public bool IsTurboMode;
    public GameObject btnCancel;
    public bool isContinuePlay;
    [SerializeField] List<Sprite> spriteTurbo;
    [SerializeField] List<Image> imgBtn;
    public TextMeshProUGUI txtSpin;
    //[SerializeField] Image backLight;

    GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        btnCancel.SetActive(false);
        IsAutoPlay = false;
        IsTurboMode = false;
        isContinuePlay = false;
        CheckAutoPlay();
        CheckTurboMode();

    }
    public void PressBtnAutoPlay()
    {
        IsAutoPlay = !IsAutoPlay;
        CheckAutoPlay();
        isContinuePlay = false;
        btnCancel.SetActive(false);
    }
    public void PressBtnTurbo()
    {
        IsTurboMode = !IsTurboMode;
        CheckTurboMode();
        isContinuePlay = false;
        btnCancel.SetActive(false);
    }
    void CheckAutoPlay()
    {
        if (IsAutoPlay)
        {
            imgBtn[0].sprite = spriteTurbo[0];
            imgBtn[0].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
            //if (backLight != null) backLight.sprite = spriteTurbo[2];
        }
        else
        {
            imgBtn[0].sprite = spriteTurbo[1];
            imgBtn[0].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
            //if (backLight != null) backLight.sprite = spriteTurbo[3];
        }
        ModeButton();
        LeanTween.scale(gm.btnAnim, Vector2.one, .2f).setEase(LeanTweenType.easeOutElastic);
    }

    void CheckTurboMode()
    {
        if (IsTurboMode)
        {
            imgBtn[1].sprite = spriteTurbo[0];
            imgBtn[1].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
            //if (backLight != null) backLight.sprite = spriteTurbo[2];
        }
        else
        {
            imgBtn[1].sprite = spriteTurbo[1];
            imgBtn[1].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
            //if (backLight != null) backLight.sprite = spriteTurbo[3];
        }
        ModeButton();
        LeanTween.scale(gm.btnAnim, Vector2.one, .2f).setEase(LeanTweenType.easeOutElastic);
    }

    public void ModeButton()
    {
        if (IsTurboMode&&!IsAutoPlay)
        {
            txtSpin.text = "Turbo";
        }
        else if(!IsTurboMode&IsAutoPlay)
        {
            txtSpin.text = "Auto";
        }
        else if (IsTurboMode&IsAutoPlay)
        {
            txtSpin.text = "Turbo & Auto";
        }
        else
        {
            txtSpin.text = "Spin";
        }
    }

    public void StopAllMode()
    {
        //if (IsAutoPlay)
        //{
        //    if (state == AnimationGameState.wheel2)
        //    {
        //        if (IsTurboMode)
        //        {
        //            toggleTurbo.Switching();
        //        }
        //        if (IsAutoPlay)
        //        {
        //            toggleAuto.Switching();
        //        }
        //    }
        //    IsAutoPlay = false;
        //    IsTurboMode = false;
        //    CheckAutoPlay();
        //    CheckTurboMode();
        //}
        //StopAllCoroutines();
        btnCancel.SetActive(false);
        isContinuePlay = false;
        ModeButton();
        LeanTween.scale(gm.btnAnim, Vector2.one, .2f).setEase(LeanTweenType.easeOutElastic);

    }
}


