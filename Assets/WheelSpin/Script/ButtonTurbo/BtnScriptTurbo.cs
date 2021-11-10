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
        }
        else
        {
            imgBtn[0].sprite = spriteTurbo[1];
            imgBtn[0].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
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
        }
        else
        {
            imgBtn[1].sprite = spriteTurbo[1];
            imgBtn[1].gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
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

        btnCancel.SetActive(false);
        isContinuePlay = false;
        ModeButton();
        LeanTween.scale(gm.btnAnim, Vector2.one, .2f).setEase(LeanTweenType.easeOutElastic);

    }
}


