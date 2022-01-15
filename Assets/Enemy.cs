using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Player.OnAttacked?.Invoke("HIGH");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Player.OnAttacked?.Invoke("LOW");
        }
    }
}
