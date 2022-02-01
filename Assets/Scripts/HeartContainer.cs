using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    List<GameObject> refs;

    private void Awake()
    {
        refs = new List<GameObject>();
    }

    public void Add()
    {
        GameObject obj = Instantiate(heartPrefab, transform);
        refs.Add(obj);
    }

    public void Remove()
    {
        GameObject lastHeart = refs[refs.Count - 1];
        Destroy(lastHeart);
        refs.Remove(lastHeart);
    }

    public void Clear()
    {
        refs.ForEach(Destroy);
        refs.Clear();
    }
}
