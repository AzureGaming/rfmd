using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDamageOverlay : MonoBehaviour
{
    Image image;
    Color origColor;

    private void OnEnable()
    {
        Player.OnHit += ShowOverlay;
    }

    private void OnDisable()
    {
        Player.OnHit -= ShowOverlay;
    }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        origColor = image.color;
        origColor.a = 0.3f;
    }

    void ShowOverlay()
    {
        StartCoroutine(FadeToZeroAlpha(1.25f));
    }

    IEnumerator FadeToZeroAlpha(float t)
    {
        image.color = origColor;
        while (image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
