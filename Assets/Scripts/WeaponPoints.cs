using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponPoints : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetText(float points)
    {
        text.text = $"Weapon Points: {points}";
    }
}
