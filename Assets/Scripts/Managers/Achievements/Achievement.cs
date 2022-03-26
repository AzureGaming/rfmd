using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Achievement : MonoBehaviour
{
    protected string title;
    protected string description;
    public bool achieved;

    public void UpdateCompletion()
    {
        if (achieved)
        {
            return;
        }

        if (RequirementsMet())
        {
            achieved = true;
        }
    }

    bool RequirementsMet()
    {
        return Requirement();
    }

    abstract public bool Requirement();
}