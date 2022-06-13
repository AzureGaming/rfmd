using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] ShopItemData shotgunUpgrade;

    private void Start()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        if (shotgunUpgrade.isOwned)
        {
            FindObjectOfType<GameManager>().UpgradePlayerDamage();
        }
    }
}
