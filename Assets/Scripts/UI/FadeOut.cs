using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(FadeTextToZeroAlpha(1f));
    }

    public IEnumerator FadeTextToZeroAlpha(float t)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
