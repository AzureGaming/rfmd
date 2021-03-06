using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeAmount;
    private void OnEnable()
    {
        Boss.OnDamaged += Shake;
    }

    private void OnDisable()
    {
        Boss.OnDamaged -= Shake;
    }

    void Shake(int health, int maxHealth)
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
        Camera.main.transform.localPosition = origPos;
    }
}
