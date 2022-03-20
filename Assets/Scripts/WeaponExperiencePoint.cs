using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponExperiencePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            GameManager.OnPickupExperiencePoint?.Invoke();
            Destroy(gameObject);
        }
    }
}
