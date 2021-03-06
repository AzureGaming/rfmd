using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void Hit();
    public static Hit OnHit;
    public delegate void Jumped();
    public static Jumped OnJumped;
    public delegate void Dodged(Enemy enemyRef);
    public static Dodged OnDodged;
    public delegate void Death();
    public static Death OnDeath;

    public PlayerAnimation animation;
    public PlayerAudio audio;

    public float jumpForce = 5f;

    public bool isDead { get; private set; } = false;

    [SerializeField] bool isGodMode = false;
    bool grounded = false;
    bool isInvincible = false;
    bool isComboAttacking = false;
    Coroutine jumpRoutine;
    Coroutine slideRoutine;
    Coroutine invincibleRoutine;
    Rigidbody2D rb;
    GameManager gameManager;

    private void OnEnable()
    {
        Enemy.OnImpact += HandleAttacked;
        TouchManager.OnSwipeUp += HandleJump;
        TouchManager.OnSwipeDown += HandleSlide;
    }

    private void OnDisable()
    {
        Enemy.OnImpact -= HandleAttacked;
        TouchManager.OnSwipeUp -= HandleJump;
        TouchManager.OnSwipeDown -= HandleSlide;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();

        gameManager.isPlayerAlive = true;
    }

    private void Start()
    {
        audio.PlayRun();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            HandleJump();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            HandleSlide();
        }

        PlayAnimations();
    }

    private void FixedUpdate()
    {
        IsGrounded();
    }

    void HandleJump()
    {
        Debug.Log("JUMP{");
        if (IsActionValid())
        {
            jumpRoutine = StartCoroutine(Jump());
        }
    }

    void HandleSlide()
    {
        if (IsActionValid())
        {
            slideRoutine = StartCoroutine(Slide());
        }
    }

    void HandleAttacked(Enemy.AttackType attackType, Enemy enemyRef)
    {
        if (invincibleRoutine != null)
        {
            return;
        }

        bool damagedHigh = attackType == Enemy.AttackType.High && slideRoutine == null;
        bool damagedLow = attackType == Enemy.AttackType.Low && jumpRoutine == null;

        if (damagedHigh || damagedLow)
        {
            TakeDamage();
        }
        else
        {
            if (attackType == Enemy.AttackType.High)
            {
                animation.PlaySlideAttack();
            }
            else
            {
                animation.PlayJumpAttack();
            }
            Dodge(enemyRef);
        }
    }

    void Dodge(Enemy enemyRef)
    {
        audio.PlayDodge();
        OnDodged?.Invoke(enemyRef);
    }

    void TakeDamage()
    {
        if (isGodMode)
        {
            return;
        }

        OnHit?.Invoke();
        if (gameManager.lives < 1)
        {
            Die();
            return;
        }
        animation.PlayHurt();
    }

    IEnumerator Slide()
    {
        animation.PlaySlide(true);
        audio.StopRun();
        audio.PlaySlide();

        yield return new WaitForSeconds(0.5f);

        animation.PlaySlide(false);
        slideRoutine = null;
        audio.PlayRun();
    }

    IEnumerator Jump()
    {
        OnJumped?.Invoke();

        float invincibleTime = 0.3f;
        float timeElapsed = 0f;
        audio.StopRun();

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animation.PlayJump();
        audio.PlayJump();
        while (timeElapsed <= invincibleTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => grounded);
        jumpRoutine = null;
        audio.PlayRun();
    }

    void Die()
    {
        if (!isDead) // prevent dying when dead
        {
            isDead = true;
            OnDeath?.Invoke();
            animation.PlayDeath();
            audio.PlayDeath();
            FindObjectOfType<GameManager>().isPlayerAlive = false;
        }
    }

    bool IsActionValid()
    {
        return !isDead && grounded && slideRoutine == null && invincibleRoutine == null && !isComboAttacking;
    }

    void IsGrounded()
    {
        float rayLength = 0.25f;
        Debug.DrawRay(transform.position, -Vector2.up * rayLength, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayLength, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void PlayAnimations()
    {
        animation.SetYVelocity(Mathf.Round(rb.velocity.y));
        animation.SetGrounded(grounded);
    }
}
