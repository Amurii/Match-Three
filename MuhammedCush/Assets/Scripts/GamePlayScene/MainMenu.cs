using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] TextMeshProUGUI Coins;
    void Start()
    {
       // Coins.SetText("Coins:"+GameManager.instance.GetCoins().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadShopScene()
    {
        GameManager.instance.LoadSceenCanvas(GameManager.instance.Shop);
    }
    public void LoadSceen()
    {
        //Loading gameplayed scene
        GameManager.instance.LoadSceenCanvas(GameManager.instance.GamePlay);
    }
    private void OnEnable()
    {
      //  Loading.instance.transform.gameObject.SetActive(true);
    }
}
