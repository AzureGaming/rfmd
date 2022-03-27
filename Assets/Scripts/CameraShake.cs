using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeAmount;
    private void OnEnable()
    {
        Enemy.OnTakeDamage += Shake;
    }

    private void OnDisable()
    {
        Enemy.OnTakeDamage -= Shake;
    }

    void Shake(int val)
    {
        StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float timeElapsed = 0f;
        float totalTime = 0.1f;
        Vector3 origPos = Camera.main.transform.localPosition;
        while (timeElapsed <= totalTime)
        {
            Vector3 newPos = Camera.main.transform.localPosition;
            Vector3 offset = Random.insideUnitSphere * shakeAmount;
            newPos.x = offset.x;
            newPos.y = offset.y;
            Camera.main.transform.localPosition = newPos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Done shake");
        Camera.main.transform.localPosition = origPos;
    }
}
