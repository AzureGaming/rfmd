using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPosition : MonoBehaviour
{
    public float timeToReachTarget = 1f;
    public Vector3 endPos;
    float time;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPos, endPos, time);
    }
}
