using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.OnPickupWeaponPoint?.Invoke();
            Destroy(gameObject);
        }
    }
}
