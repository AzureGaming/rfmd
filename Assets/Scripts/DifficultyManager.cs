using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] public int level { get; private set; } = 1;
    [SerializeField] int level2Threshold = 100;
    [SerializeField] int level3Threshold = 200;

    private void Update()
    {
        //if (scoreManager.score >= level2Threshold && scoreManager.score <= level3Threshold)
        //{
        //    level = 2;
        //}
    }
}
