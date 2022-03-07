using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnHover : MonoBehaviour, IPointerEnterHandler
{
    AudioManager audio;

    private void Awake()
    {
        audio = FindObjectOfType<AudioManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audio.Play("Button_Hover");
    }
}
