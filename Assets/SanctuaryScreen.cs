using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SanctuaryScreen : MonoBehaviour
{
    [SerializeField] GameObject shopMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject rootMenu;
    [SerializeField] TMP_Text headerText;
    [SerializeField] TMP_Text totalCurrency;
    PlayerStatsTracker stats;

    private void Awake()
    {
        stats = FindObjectOfType<PlayerStatsTracker>();
    }

    private void OnEnable()
    {
        headerText.text = "";
        rootMenu.SetActive(true);
        shopMenu.SetActive(false);
    }

    private void Update()
    {
        totalCurrency.text = stats.GetTotalCurrency().ToString();
    }

    public void GoBack()
    {
        if (shopMenu.activeSelf)
        {
            rootMenu.SetActive(true);
            shopMenu.SetActive(false);
        } else
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void OpenShopMenu()
    {
        headerText.text = "Hellforge";
        rootMenu.SetActive(false);
        shopMenu.SetActive(true);
    }
}
