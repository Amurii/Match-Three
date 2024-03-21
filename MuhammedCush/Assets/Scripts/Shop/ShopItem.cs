using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Sprite image;
    [SerializeField] public int Price;
    [SerializeField] public int index;
    [SerializeField] public BoosterAbility BoosterAbility;
    public bool HasAbility;

    [Header("Item Settings")]
    [SerializeField] TextMeshProUGUI PriceTxt;
    [SerializeField] Button purchaseBtn;

    private void OnEnable()
    {
        PriceTxt.text = Price.ToString();
    }
    public void PurchaseItem(ShopItem item)
    {
        MainShop.instance.PurchaseItem(item);
    }
}
