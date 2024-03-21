using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSetting : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }
    private void OnEnable()
    {
        LeanTween.scale(this.gameObject, new Vector3(1, 1, 1), 0.8f).setEase(LeanTweenType.easeInOutQuart);
    }
    public void OnClose()
    {
        // this.gameObject.SetActive(true);
        LeanTween.scale(this.gameObject, new Vector3(0, 0, 0), 0.25f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(()=>gameObject.SetActive(false));
    }
}
