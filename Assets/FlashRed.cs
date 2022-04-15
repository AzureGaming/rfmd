using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashRed : MonoBehaviour
{
    SpriteRenderer spriteR;
    Color origColor;

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        origColor = spriteR.color;
    }

    public void RunRoutine()
    {
        StartCoroutine(FlashRedRoutine());
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
