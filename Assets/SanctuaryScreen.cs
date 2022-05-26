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

    private void OnEnable()
    {
        headerText.text = "";
        rootMenu.SetActive(true);
        shopMenu.SetActive(false);
    }

    public void GoBack()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenShopMenu()
    {
        headerText.text = "Hellforge";
        rootMenu.SetActive(false);
        shopMenu.SetActive(true);
    }
}
