using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] TMP_Text description;
    [SerializeField] TMP_Text price;
    [SerializeField] Image icon;
    [SerializeField] Button buyButton;
    bool isOwned;
    ShopItemData dataRef;
    PlayerStatsTracker stats;
    Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
        stats = FindObjectOfType<PlayerStatsTracker>();
    }

    private void Update()
    {
        if (isOwned)
        {
            background.color = Color.green;
            buyButton.interactable = false;
        }
    }

    public void RegisterData(ShopItemData data)
    {
        dataRef = data;
        label.text = data.label;
        description.text = data.description;
        price.text = $"Price: {data.price}";
        icon.sprite = data.icon;
        isOwned = data.isOwned;
    }

    public void Buy()
    {
        isOwned = true;
        stats.SubtractTotalCurrency(dataRef.price);
        dataRef.isOwned = isOwned;
        UpgradeManager mnger = FindObjectOfType<UpgradeManager>();
        if (mnger)
        {
            mnger.UpdateState();
        }
    }
}
