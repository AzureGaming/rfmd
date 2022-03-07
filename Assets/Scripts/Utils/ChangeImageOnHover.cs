using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeImageOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite sourceImage;
    Image image;
    Sprite orig;

    private void Awake()
    {
        image = GetComponent<Image>();
        orig = image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = sourceImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = orig;
    }
}
