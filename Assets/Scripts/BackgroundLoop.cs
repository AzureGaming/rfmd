using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    bool hasLooped = false;
    Bounds cameraBounds;
    SpriteRenderer spriteR;

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
        cameraBounds = GetOrthographicBounds();
    }

    private void Update()
    {
        if (transform.position.x <= cameraBounds.min.x && !hasLooped)
        {
            hasLooped = true;
            Instantiate(gameObject, GetSpawnPos(), Quaternion.identity, transform.parent);
        }

        if (transform.position.x + spriteR.bounds.size.x <= cameraBounds.min.x)
        {
            Destroy(gameObject);
        }
    }

    Bounds GetOrthographicBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        Bounds bounds = new Bounds(
            Camera.main.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    Vector3 GetSpawnPos()
    {
        Vector3 spawnPos = new Vector3(spriteR.bounds.max.x + spriteR.bounds.size.x / 2, 0f);
        return spawnPos;
    }
}
