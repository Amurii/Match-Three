using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject SoundSettings;

    [Header("Sound")]
    [SerializeField] Image soundImage;
    [SerializeField] Sprite SoundOnImage;
    [SerializeField] Sprite SoundOffImage;

    public static BackGroundMenu instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckSoundImage();
    }
    public void SoundOnOff()
    {
        if (!SoundSettings.activeInHierarchy)
            SoundSettings.SetActive(!SoundSettings.activeInHierarchy);
        else SoundSettings.GetComponent<SoundSetting>().OnClose();
    }
    private void CheckSoundImage()
    {
        if (SoundManager.instance.SoundOn)
        {
            //Set icon of sound on
            soundImage.sprite = SoundOnImage;
        }
        else
        {
            //Set icon of sound off
            soundImage.sprite = SoundOffImage;
        }
    }
    public void ShopButton()
    {
        GameManager.instance.LoadSceenCanvas(GameManager.instance.Shop);
    }
}
