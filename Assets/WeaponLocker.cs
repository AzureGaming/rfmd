using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLocker : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Enemy.OnTakeDamage?.Invoke(gameManager.GetDamage());
        }
    }
}
