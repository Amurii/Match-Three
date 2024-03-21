using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI MovesTxt;
    [SerializeField] TextMeshProUGUI TargerTxt;
    [SerializeField] TextMeshProUGUI LevelTxt;
    [SerializeField] TextMeshProUGUI CoinTxt;

    [Header("Pop")]
    [SerializeField] GameObject WiningPopObj;
    [SerializeField] GameObject Revive;
   public int Moves;
     int Target;
    int level;
    public bool isPlayed;
    int itemDestroyedValue;
    public static GamePlay instance;
    public static int coins;
    private void Start()
    {
        coins = 0;
    }
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        coins = 0;
        WiningPopObj.SetActive(false);
        isPlayed = true;
        Target = GameManager.instance.GetTarget();
        Moves = GameManager.instance.GetMoves();
       // Target = GameManager.instance.GetTarget();
        level= GameManager.instance.GetLevel();
        CoinTxt.text = coins.ToString();
        MovesTxt.text = Moves.ToString();
        TargerTxt.text = Target.ToString();
        LevelTxt.text = level.ToString();
        itemDestroyedValue = GameManager.instance.GetItemDestoyValue();
        CheckMoves();
    }
    void CheckMoves()
    {
        if (GameManager.instance.GetMoves() == 0)
        {
            //There is no moves
            Revive.SetActive(true);
        }
        else
        {
            //Contnue
        }
    }
    public void ReturnToMainMenu()
    {
        WiningPop.isLevelUp = false;
        if(!WiningPopObj.activeSelf)
        WiningPopObj.SetActive(true);
    }
    public int GetTarget() => Target;
    public void ItemMove()
    {
       
            Moves--;
            MovesTxt.text = Moves.ToString();
           // SaveManager.instance.state.Moves = Moves;
            //SaveManager.instance.Save();
            if (Moves == 0)
            {
            //Game over
            Revive.SetActive(true);
                //if(CurrentTarget>0)
               // GameManager.instance.LoadSceenCanvas(GameManager.instance.GameOver);
            }

    }
    public void TargetUpdate(int DestroyedCounter)
    {
        itemDestroyedValue = GameManager.instance.GetItemDestoyValue();
        if (!isPlayed) return;
        for (int i = 0; i < DestroyedCounter; i++)
        {
            if (i >3)
            {
                itemDestroyedValue += 5; 
            }
            Target -= itemDestroyedValue;
            AddCoin(itemDestroyedValue);
            if (Target >= 0)
            {
                SaveManager.instance.state.CurrentTarget = Target;
                SaveManager.instance.Save();
            }
        }
        if (Target <= 0)
        {
            Target = 0;
            //CurrentTarget access
            if(WiningPopObj.activeInHierarchy)return;
              WiningPop.isLevelUp = true;
              WiningPopObj.SetActive(true);
            //WiningPop.isLevelUp = false;
        }
        TargerTxt.text = Target.ToString();
        
    }
    public int GetCoin ()=> coins;

    public void AddCoin(int added) { coins += added; CoinTxt.text = coins.ToString(); }

}
