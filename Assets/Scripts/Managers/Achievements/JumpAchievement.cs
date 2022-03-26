using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAchievement : Achievement
{
    [SerializeField] int jumps = 0;

    private void Start()
    {
        title = "some title";
    }

    public override bool Requirement()
    {
        return jumps >= 50;
    }
}
