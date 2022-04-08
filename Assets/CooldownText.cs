using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CooldownText : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetText(float cooldown)
    {
        text.text = $"{cooldown}";
    }
}
