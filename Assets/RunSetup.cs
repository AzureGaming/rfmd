using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSetup : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    const float GROUND_Y = -0.415712f;

    public IEnumerator IntroAnimation()
    {
        if (FindObjectOfType<Player>() == null)
        {
            Instantiate(playerPrefab);
        }

        Transform playerTransform = FindObjectOfType<Player>().transform;
        Vector3 leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        float playerPosOffset = 0.5f; // do not sit directly on the camera bound
        playerTransform.position = new Vector2(leftBound.x - playerPosOffset, GROUND_Y);

        GameObject targetObj = GameObject.FindGameObjectWithTag("PlayerPosition");
        if (targetObj == null)
        {
            Debug.LogWarning("Player position is not in scene!");
            yield break;
        }

        Vector2 targetPos = new Vector2(targetObj.transform.position.x, GROUND_Y);
        yield return StartCoroutine(MoveToTarget(playerTransform, targetPos));
    }

    IEnumerator MoveToTarget(Transform t, Vector2 target)
    {
        float timeElapsed = 0f;
        float totalTime = 1.5f;
        Vector2 startPos = t.position;
        while (timeElapsed <= totalTime)
        {
            Vector2 newPos = Vector2.Lerp(startPos, target, timeElapsed / totalTime);
            t.position = newPos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        t.position = target;
    }
}
