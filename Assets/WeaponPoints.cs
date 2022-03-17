using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponPoints : MonoBehaviour
{
    TMP_Text text;
    GameManager gameManager;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        text.text = $"Weapon Points: {gameManager.weaponPoints}";
    }
}
