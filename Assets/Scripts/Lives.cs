using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lives : MonoBehaviour
{
    HeartContainer heartContainer;
    private void Awake()
    {
        heartContainer = GetComponentInChildren<HeartContainer>();
    }

    public void Init(int lives)
    {
        heartContainer.Clear();
        SetHearts(lives);
    }

    public void SetHearts(int lives)
    {
        heartContainer.Clear();
        for (int i = 0; i < lives; i++)
        {
            heartContainer.Add();
        }
    }
}
