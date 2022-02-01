using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    [SerializeField] float speed = 0f;

    private void Update()
    {
        Vector3 dir = transform.right;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
