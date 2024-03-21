using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    Postion,
    Scale,
}
public class SpawnUI : MonoBehaviour
{
    [Header("UI Settings")]
    public SpawnType SpawnType;
    public LeanTweenType AnimationType;
    public Vector3 From;
    public Vector3 To;
    public float Duration;
    private void OnEnable()
    {
        if (SpawnType == SpawnType.Scale)
        {
            transform.localScale = From;
            LeanTween.scale(gameObject, To, Duration).setEase(AnimationType);
        }
        if(SpawnType == SpawnType.Postion)
        {

            transform.position = From;
            LeanTween.move(gameObject, To, Duration).setEase(AnimationType);
        }
    }
    public void Close()
    {
        if (SpawnType == SpawnType.Scale)
        {
            transform.localScale = To;
            LeanTween.scale(gameObject, From, Duration).setEase(AnimationType).setOnComplete(OnComplete); ;
        }
        if (SpawnType == SpawnType.Postion)
        {

            transform.position = To;
            LeanTween.move(gameObject, From, Duration).setEase(AnimationType).setOnComplete(OnComplete);
        }
    }
    private void OnComplete()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
