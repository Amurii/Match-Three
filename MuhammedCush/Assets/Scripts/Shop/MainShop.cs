using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainShop : MonoBehaviour
{

    //Singlton
    public static MainShop instance;

    [Header("Coins")]
    [SerializeField] TextMeshProUGUI TotalCoinsTxt;
    [SerializeField] GameObject Pop;
    private int TotalCoins;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
       
    }
    private void OnEnable()
    {
       GetComponent<RectTransform>().transform.localScale = new Vector3(0, 0, 0);
        GetComponent<RectTransform>().transform.LeanScale(new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeInOutQuart);
        TotalCoins = GameManager.instance.GetAllCoins();
        TotalCoinsTxt.text = TotalCoins.ToString();
    }
    public void ReturnToMainMenu()
    {
        GameManager.instance.LoadSceenCanvas(GameManager.instance.MainMenu);
    }
    public void PurchaseItem(ShopItem item)
    {
        if (TotalCoins <= 0 || TotalCoins < item.Price)
        {
            //Show the pop that there is no enough money
            Pop.SetActive(true);
            return;
        }
        //Check if already in JSON or not
        //Check 

        //After checked 
        Booster boostItem = new Booster();
        boostItem.index = item.index;
        boostItem.icon = item.image;
        boostItem.BoosterAbility = item.BoosterAbility;
        GameManager.instance.PurchaseItem(boostItem,item.Price);
        TotalCoins = GameManager.instance.GetAllCoins();
        TotalCoinsTxt.text = TotalCoins.ToString();
    }
}
