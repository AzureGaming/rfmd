using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] GameObject shopItemPrefab;
    [SerializeField] ShopItemData shotgunData;
    Transform container;

    private void Awake()
    {
        container = GameObject.FindGameObjectWithTag("ShopContainer").transform;
    }

    private void Start()
    {
        CreateShopItem(shotgunData);
    }

    void CreateShopItem(ShopItemData data)
    {
        ShopItem shopItem = Instantiate(shopItemPrefab, container).GetComponent<ShopItem>();
        shopItem.RegisterData(data);
    }
}
