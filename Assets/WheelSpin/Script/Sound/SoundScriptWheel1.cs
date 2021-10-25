using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundScriptWheel1 : MonoBehaviour
{
    [SerializeField] List<Sprite> spriteBtn;
    [SerializeField] Button btnSound;

    bool isOn;


    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        isOn = true;
        //btnSound.onClick.AddListener(delegate { SetSound(); });
    }

    public void SetSound()
    {
        isOn = !isOn;
        if (isOn)
        {
            audioSource.mute = false;
            btnSound.GetComponent<Image>().sprite = spriteBtn[0];
        }
        else
        {
            audioSource.mute = true;
            btnSound.GetComponent<Image>().sprite = spriteBtn[1];
        }
    }
}
