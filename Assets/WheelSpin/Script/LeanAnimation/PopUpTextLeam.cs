using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpTextLeam : MonoBehaviour
{
    private void Awake()
    {
        gameObject.transform.localScale = Vector3.zero;
    }
    private void Start()
    {
        Finish();
    }

    void Finish()
    {
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f), .4f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveLocalY(gameObject, -20, .15f);
        LeanTween.scale(gameObject, new Vector3(1f, 1f), 1f).setDelay(.5f).setEase(LeanTweenType.easeInCirc).setOnComplete(() => Destroy(gameObject));
        LeanTween.moveLocalY(gameObject, 200, 1.5f).setDelay(.3f).setEase(LeanTweenType.easeInCirc);
        //LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0.1f, 1f);

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        var color = text.color;
        color.a = .5f;
        var fadeoutcolor = color;
        fadeoutcolor.a = 1;
        LeanTween.value(gameObject, updateValueExampleCallback, fadeoutcolor, color, 1f).setDelay(.5f);
    }

    void updateValueExampleCallback(Color val)
    {
        gameObject.GetComponent<TextMeshProUGUI>().color = val;
    }
}
