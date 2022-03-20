using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponLevel : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetText(float val)
    {
        text.text = $"Weapon Level: {val}";
    }
}
