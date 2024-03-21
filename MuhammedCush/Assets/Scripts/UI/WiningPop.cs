using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PopType
{
    Win,
    Fail
}
public class WiningPop : MonoBehaviour 
{

    [SerializeField] TextMeshProUGUI MoveTxt;
    [SerializeField] TextMeshProUGUI CoinsTxt;

    [SerializeField] TextMeshProUGUI LevelUp;
    [SerializeField] TextMeshProUGUI NextLevelBtn;
    [SerializeField] GameObject Ads;
    public static PopType popType;
    public static bool isActive;
    public static bool isLevelUp;
    private void OnEnable()
    {
        if (isActive) return;
        Ads.SetActive(true);
        GamePlay.instance.isPlayed = false;
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.clamp);
        CoinsTxt.text = "COINS: "+ GamePlay.instance.GetCoin().ToString();
        if (isLevelUp)
        {
            GameManager.instance.NextLevel();
            // CoinsTxt.text = "Moves:" + GameManager.instance.GetPreviusLevelCoins();
            CoinsTxt.text = "COINS: " + GamePlay.instance.GetCoin().ToString();
            SoundManager.instance.AudioPlay(AudioClips.Winning);
            LevelUp.text = "Level UP!";
            NextLevelBtn.text = "Next Level";
            // GameManager.instance.LoadSceenCanvas(GameManager.instance.GamePlay);
        }
        else
        {
            LevelUp.text = "GAME OVER!";
            NextLevelBtn.text = "RETRY";
        }
        GameManager.instance.RemoveBoostersAfterSessionComplete();

    }
    public void OnNextLevel(string action)
    {
        GameManager.instance.SaveCoin(GameManager.instance.GetAllCoins() + GamePlay.coins);
        GamePlay.coins = 0;
        GamePlay.instance.isPlayed = true;
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.3f).setEase(LeanTweenType.clamp).setOnComplete(OnComplete =>
        {

            StartCoroutine(ActiveFalse(action));
        });
      
       


    }
    IEnumerator ActiveFalse(string action)
    {
        yield return new WaitForSeconds(0.1f);
        if (action == "NextLevel")
        {
            GameManager.instance.LoadSceenCanvas(GameManager.instance.GamePlay);
            //Save the used booster in this session
           
        }
        else GameManager.instance.LoadSceenCanvas(GameManager.instance.MainMenu);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
    public void OnAdsClick()
    {
        GamePlay.coins *= 3;
        CoinsTxt.text = "COINS: " + GamePlay.instance.GetCoin().ToString();
        Ads.SetActive(false);
    }
    private void OnDisable()
    {
        isActive=false;
    }

}
