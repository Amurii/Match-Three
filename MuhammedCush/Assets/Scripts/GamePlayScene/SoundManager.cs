using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AudioClips
{
    Fail,
    Bomb,
    ItemsFall,
    Winning
}
public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Sound Setting")]
    [SerializeField] AudioSource MainSource;
    public bool SoundOn;
    public float Voulume;

    [Header("Audio Clips")]
    [SerializeField] AudioClip failClip;
    [SerializeField] AudioClip fallSound;
    [SerializeField] AudioClip WiningClip;

    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ChangeVoulume(Voulume);
       // BackGroundMenu.instance.SoundOnOff();
    }
    public void ChangeVoulume(float vol)
    {
        if (vol > 1)
            vol = 1;
        Voulume = vol; ;
        MainSource.volume = Voulume;
    }
    public void SoundOnOff()
    {
        SoundOn = !SoundOn;
        MainSource.enabled = SoundOn;
        SaveManager.instance.state.IsSoundOn = SoundOn;
        SaveManager.instance.Save();
    }
    public void AudioPlay(AudioClips audioClips)
    {
        if (SoundOn)
        {
            switch (audioClips)
            {
                case AudioClips.Fail:
                    MainSource.PlayOneShot(failClip);
                    break;
                case AudioClips.Bomb:
                    break;
                case AudioClips.ItemsFall:
                    PlaySound(fallSound);
                    break;
                case AudioClips.Winning:
                    PlaySound(WiningClip);
                    break;
            }
        }
    }

    private void PlaySound(AudioClip audioClip )
    {
        if (SoundOn&&!MainSource.isPlaying)
            MainSource.PlayOneShot(audioClip);
    }
}
