using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RevivePop : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI Moves;
    [SerializeField] GameObject WiningPopObj;
    int moves;
    private void OnEnable()
    {
        GamePlay.instance.isPlayed = false;
        Moves.text = "0 MOVES";
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.clamp);
    }
    public void OnAdsClick()
    {
        // SaveManager.instance.state.Moves = 5;
        //SaveManager.instance.Save();
        //GameManager.instance.LoadData();
        GamePlay.instance.Moves += 5;
      
        OnClose("Revived");
    }
    public void OnNoThanksClick()
    {
        OnClose("Quit");
    }
    public void OnClose(string action)
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.8f).setEase(LeanTweenType.clamp).setOnComplete(OnComplete=> {
            if (action == "Quit")
            {
                WiningPop.isLevelUp = false;
                WiningPopObj.SetActive(true);
            }
            else 
            {
                GameManager.instance.SetTarget(GamePlay.instance.GetTarget());
                GameManager.instance.LoadSceenCanvas(GameManager.instance.GamePlay); 
            }
            gameObject.transform.parent.gameObject.SetActive(false);
        });
        GamePlay.instance.isPlayed = true;

    }
    void OnComplete()
    {
        
        
    }
}
