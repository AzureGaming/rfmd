using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAchievement : Achievement
{
    public int jumps = 0;

    public override bool Requirement()
    {
        return jumps >= 50;
    }
}
