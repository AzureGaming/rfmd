using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlashRed : MonoBehaviour
{
    SpriteRenderer spriteR;
    Color origColor;

    private void OnEnable()
    {
        GameManager.OnDamageEnemy += HandleTakeDamage;
    }

    private void OnDisable()
    {
        GameManager.OnDamageEnemy -= HandleTakeDamage;
    }

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        origColor = spriteR.color;
    }

    void HandleTakeDamage(int damage)
    {
        if (damage < GetComponent<Enemy>().health)
        {
            StartCoroutine(FlashRedRoutine());
        }
    }

    IEnumerator FlashRedRoutine()
    {
        float timeElapsed = 0f;
        float totalTime = 0.001f;
        Color redColor = Color.red;
        redColor.a = 50;

        while (timeElapsed <= totalTime)
        {
            if (spriteR.color == origColor)
            {
                spriteR.color = redColor;
            }
            else
            {
                spriteR.color = origColor;
            }
            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.15f);
        }
        spriteR.color = origColor;
    }

}
