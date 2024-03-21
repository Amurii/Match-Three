using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterManager : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] BoosterItem boostPrefab;
    public  ShopItem [] OriginalBoostersSprite;
    public Dictionary<int,BoosterItem> allBoosters = new Dictionary<int,BoosterItem>();
    public Dictionary<int, BoosterItem> boostersUsedInSession = new Dictionary<int, BoosterItem>();
    public static BoosterManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
       

    }
    private void OnEnable()
    {
        ReEnable();
        Setup();
    }
    void Setup()
    {
        for (int i = 0; i < GameManager.instance.BoosterItemsJSON.Count; i++)
        {
            BoosterItem temp = Instantiate(boostPrefab, content);
            temp.boosterAbility = GameManager.instance.BoosterItemsJSON[i].BoosterAbility;
            temp.itemIndex = GameManager.instance.BoosterItemsJSON[i].index;
            temp.itemCout= GameManager.instance.BoosterItemsJSON[i].count;

            foreach(var item in OriginalBoostersSprite)
            {
                if (temp.itemIndex == item.index)
                    temp.image.sprite = item.image;
            }
            allBoosters.Add(temp.itemIndex,temp);
        }
    }
    private void OnDisable()
    {
        ReEnable();
    }
    void ReEnable()
    {
        //Reset all booster after disable or re-enable
        foreach (var booster in allBoosters)
        {
            if (booster.Value != null)
                Destroy(booster.Value.gameObject);
        }
        allBoosters.Clear();
    }
    public void SaveBoosterInSaveData()
    {
        //GameManager.instance.BoosterItemsJSON
    }
}
[Serializable]
public class Booster
{
    public int index;
    public int count;
    public int iconIndex;
    public Sprite icon;
    public BoosterAbility BoosterAbility;
}
