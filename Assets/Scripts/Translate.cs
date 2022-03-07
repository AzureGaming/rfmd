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

    public IEnumerator Stop()
    {
        float totalTime = 2f;
        float timeElapsed = 0f;
        while (timeElapsed <= totalTime || speed > 0f)
        {
            if (speed > 0f)
            {
                speed -= 0.001f;
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
