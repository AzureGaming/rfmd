using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.OnPickupComboPoint?.Invoke();
            Destroy(gameObject);
        }
    }
}
