using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallax = 1f;
    Transform camTransform;
    Vector3 lastCamPos;
    float textureUnitSizeX;

    private void Start()
    {
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void Update()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPos;
        transform.position += deltaMovement * parallax;
        lastCamPos = camTransform.position;

        float offset = textureUnitSizeX / 2;
        if (camTransform.position.x - transform.position.x + offset >= textureUnitSizeX)
        {
            transform.position = new Vector3(camTransform.position.x + offset, transform.position.y);
        }
        else if (transform.position.x - camTransform.position.x + offset >= textureUnitSizeX)
        {
            transform.position = new Vector3(camTransform.position.x - offset, transform.position.y);
        }
    }
}
