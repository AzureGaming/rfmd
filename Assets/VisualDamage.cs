using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDamage : MonoBehaviour
{
    SpriteRenderer spriteR;

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Boss.OnDamaged += UpdateColor;
    }

    private void OnDisable()
    {
        
        Boss.OnDamaged -= UpdateColor;
    }

    void UpdateColor(int health, int maxHealth)
    {
        if ((health / maxHealth) <= 0.25f)
        {
            spriteR.color = Color.red;
            return;
        }

        if ((health / maxHealth) <= 0.5f)
        {
            spriteR.color = Color.yellow;
            return;
        }
    }
}
