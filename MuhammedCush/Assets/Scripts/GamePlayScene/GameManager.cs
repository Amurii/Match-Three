using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("ScreensPanel")]
     public GameObject MainMenu;
     public GameObject GamePlay;
     public GameObject GameOver;
     public GameObject Shop;
     private GameObject currentSceen;
    [Header("BoardSize")]
    [SerializeField] int sizeX;
    [SerializeField] int sizeY;
  

    [Header("Coins")]
    [SerializeField] int AllCoins;

    [Header("Boosters")]
    [SerializeField] int BoosterCount;
    [SerializeField] string BoostArray;//This array in JSON 
    public  List<Booster> BoosterItemsJSON;

    [Header("Settings")]
    [SerializeField] int Moves;
    [SerializeField] int Target;
    [SerializeField] int CurrentLevel;
    [SerializeField] int ItemDestroyValue;
    [Range(1,101)] public int AbilityChanceCreated;
    public int currentLevel;
    [Header("LeanTween settings")]
    public LeanTweenType leanTweenType;
    [SerializeField] float TileMovmentSpeed=0;

    public static GameManager instance;
    //List<LeanTweenType> leanTweenTypes = new List<LeanTweenType>();
    private void Awake()
    {
        instance = this;
        ////Reset data
    // SaveManager.instance.state.Coins = 0;
        //SaveManager.instance.state.Moves = 10;
        //SaveManager.instance.state.LevelMoves = 10;
        //SaveManager.instance.state.IsSoundOn = true;
        //SaveManager.instance.state.CurrentLevel = 1;
        //SaveManager.instance.state.CurrentTarget = 250;
        //SaveManager.instance.state.LevelTarget = 100;
        //SaveManager.instance.Save();
    }
    void Start()
    {
        LoadData();
        //current scene in the start mainMenue scene
        currentSceen = MainMenu;
       // PlayerPrefs.DeleteAll();
       
    }
    public void LoadData()
    {
        //PlayerPrefs.DeleteAll();
        
        AllCoins = SaveManager.instance.state.Coins;
        Moves = SaveManager.instance.state.Moves;
        Target = SaveManager.instance.state.LevelTarget;
        currentLevel = SaveManager.instance.state.CurrentLevel;
        BoostArray = SaveManager.instance.state.BoosterItems;
        SoundManager.instance.SoundOn = SaveManager.instance.state.IsSoundOn;
        FillBoster();

    }
    private void FillBoster()
    {
        if (BoostArray.Equals(string.Empty)) return;
        JsonUt jsonUt = new JsonUt();
        jsonUt= JsonUtility.FromJson<JsonUt>(BoostArray);
        BoosterItemsJSON = jsonUt.BoosterItemsJSON;
        
    }
    public void PurchaseItem(Booster item,int price)
    {
        bool isContain=false;
            for(int i = 0; i < BoosterItemsJSON.Count; i++)
            {
                if (item.index == BoosterItemsJSON[i].index)
                {
                    BoosterItemsJSON[i].count++;
                isContain = true;
                    break;
                }
            }
        if(!isContain)
        BoosterItemsJSON.Add(item);
        JsonUt jsonUt = new JsonUt();
        jsonUt.BoosterItemsJSON = BoosterItemsJSON;
        
        string json = JsonUtility.ToJson(jsonUt);
        SaveManager.instance.state.BoosterItems = json;
        SaveManager.instance.state.Coins -= price;
        SaveManager.instance.Save();
        LoadData();
        //Save data
    }
  
    public void LoadSceenCanvas(GameObject loadingSceen)
    {
        //if (currentSceen == loadingSceen) return;
        //Current scene that refrence in the opened scene
        //loading scene refrence that the sceen need to open
        if (loadingSceen != GamePlay)
        {
            currentSceen.SetActive(false);
            loadingSceen.SetActive(true);
        }
        else
        {
            if (loadingSceen != MainMenu &&MainMenu.activeSelf)
                MainMenu.SetActive(false);
            StartCoroutine(ActiveGamePlay());
        }
        currentSceen = loadingSceen;
    }
    IEnumerator ActiveGamePlay()
    {
        yield return new WaitForEndOfFrame();
        currentSceen.SetActive(false);
        GamePlay.SetActive(true);
    }
    public int GetAllCoins()=> AllCoins;
    public Vector2 GetGridSize()=>new Vector2(sizeX, sizeY);
    public int GetMoves() => Moves;
    public int GetTarget()=> Target;
    public int GetLevel() => currentLevel;

    public int GetItemDestoyValue()=> ItemDestroyValue;

    public int GetAbilityChance()=> AbilityChanceCreated;
    public void SetTarget(int target) => Target = target;
    public float GetTileMovmentSpeed() => TileMovmentSpeed;
    public void NextLevel()
    {
       
        //coins += levelManager[currentLevel].coins;
        currentLevel++;
        Moves = SaveManager.instance.state.LevelMoves + 5;
        Target = SaveManager.instance.state.LevelTarget+50;
        SaveManager.instance.state.Coins = AllCoins;
        SaveManager.instance.state.Moves = Moves;
        SaveManager.instance.state.LevelMoves = Moves;
        SaveManager.instance.state.CurrentTarget = Target;
        SaveManager.instance.state.LevelTarget = Target;
        SaveManager.instance.state.CurrentLevel = currentLevel;
        SaveManager.instance.Save();
        LoadData();
        //Set texts
    }
    public void SaveCoin(int coin)
    {
        SaveManager.instance.state.Coins = coin;
        SaveManager.instance.Save();
        LoadData();
    }
    public string GetBoosterItems() => BoostArray;
    public void RemoveBoostersAfterSessionComplete()
    {
        BoosterItemsJSON.Clear(); 
         JsonUt jsonUt = new JsonUt();
        foreach(var item in BoosterManager.instance.allBoosters)
        {
            if (item.Value != null)
            {
                Booster booster = new Booster();
                booster.BoosterAbility = item.Value.boosterAbility;
                booster.count = item.Value.itemCout;
                booster.index = item.Value.itemIndex;
                booster.icon = item.Value.image.sprite;

                BoosterItemsJSON.Add(booster);
            }
            
        }
        jsonUt.BoosterItemsJSON = BoosterItemsJSON;

        string json = JsonUtility.ToJson(jsonUt);
        SaveManager.instance.state.BoosterItems = json;
        SaveManager.instance.Save();
        LoadData();
    }
}
[Serializable]
public class JsonUt
{
    public List<Booster> BoosterItemsJSON;

}