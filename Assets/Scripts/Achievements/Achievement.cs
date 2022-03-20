using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Achievement : MonoBehaviour
{
    public string title;
    public string description;
    public bool achieved;

    public void UpdateCompletion()
    {
        if (achieved)
        {
            return;
        }

        if (RequirementsMet())
        {
            Debug.Log($"title {title} achieved");
            achieved = true;
        }
    }

    bool RequirementsMet()
    {
        return Requirement();
    }

    abstract public bool Requirement();
}