using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotraitManager : MonoBehaviour
{
    //GameManager gm;

    //[SerializeField]GameObject historyUser;
    //[SerializeField]GameObject historyJackpot;
    //[SerializeField] GameObject clsoeButton;

    //bool isOpening;
    //private void Awake()
    //{
    //    gm = FindObjectOfType<GameManager>();
    //}
    //private void Start()
    //{
    //    isOpening = false;
    //    historyJackpot.SetActive(false);
    //    historyUser.SetActive(false);
    //    //clsoeButton.transform.localScale = Vector3.zero;
    //}
    //public void OpenHistoryUser()
    //{
    //    Close();
    //    historyUser.SetActive(true);
    //    gm.ReloadUserHistory();
    //    LeanTween.moveLocalX(historyUser, 0, .2f).setEase(LeanTweenType.easeOutCubic).setOnComplete(()=>OpenBTN());

    //}

    //public void OpenHistoryJackpot()
    //{
    //    Close();
    //    historyJackpot.SetActive(true);
    //    gm.ReloadJackPotHistory();
    //    LeanTween.moveLocalX(historyJackpot, 0, .2f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => OpenBTN());
    //}
    //void OpenBTN()
    //{
    //    clsoeButton.SetActive(true);
    //    LeanTween.scale(clsoeButton.transform.GetChild(0).gameObject, Vector2.one, .3f).setEase(LeanTweenType.easeOutQuad);
    //    isOpening = true;
    //}
    //void CloseBTN()
    //{
    //    clsoeButton.SetActive(false);
    //    LeanTween.scale(clsoeButton.transform.GetChild(0).gameObject, Vector2.zero, .3f).setEase(LeanTweenType.easeInQuad);
    //    isOpening = false;
    //}
    //public void Close()
    //{
    //    if (isOpening)
    //    {
    //        if (historyJackpot.activeInHierarchy)
    //        {
    //            LeanTween.moveLocalX(historyJackpot, 1000, .2f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => historyJackpot.SetActive(false));
    //        }
    //        else if (historyUser.activeInHierarchy)
    //        {
    //            LeanTween.moveLocalX(historyUser, 1000, .2f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => historyUser.SetActive(false));
    //        }
    //        CloseBTN();
    //    }

    //}

}
