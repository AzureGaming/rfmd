using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Attacked(string attackType);
    public static Attacked OnAttacked;

    public float jumpForce = 5f;

    bool queueJump = false;
    bool queueSlide = false;
    bool grounded = false;
    Coroutine jumpRoutine;
    Coroutine slideRoutine;
    Rigidbody2D rb;
    SpriteRenderer spriteR;

    private void OnEnable()
    {
        OnAttacked += TakeDamage;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsActionValid())
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                queueJump = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                queueSlide = true;
            }
        }
    }

    private void FixedUpdate()
    {
        IsGrounded();
        if (queueJump)
        {
            queueJump = false;
            jumpRoutine = StartCoroutine(Jump());
        }
        else if (queueSlide)
        {
            queueSlide = false;
            slideRoutine = StartCoroutine(Slide());
        }
    }

    public void TakeDamage(string attackType)
    {
        if (attackType == "HIGH")
        {
            if (!grounded)
            {
                Debug.Log("Took HIGH damage");
            }
        }
        else if (attackType == "LOW")
        {
            if (slideRoutine == null)
            {
                Debug.Log("Took LOW damage");
            }
        }
    }

    IEnumerator Slide()
    {
        spriteR.color = Color.blue;
        yield return new WaitForSeconds(0.5f);
        spriteR.color = Color.white;
        slideRoutine = null;
    }

    IEnumerator Jump()
    {
        float invincibleTime = 0.3f;
        float timeElapsed = 0f;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f); // still can get hit

        spriteR.color = Color.green;
        while (timeElapsed <= invincibleTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        spriteR.color = Color.white;
    }

    bool IsActionValid()
    {
        return grounded && slideRoutine == null;
    }

    void IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up * 2, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 2f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
