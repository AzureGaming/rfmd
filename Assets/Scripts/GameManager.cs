using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void PickupWeaponPoint();
    public static PickupWeaponPoint OnPickupWeaponPoint;
    public delegate void PickupComboPoint();
    public static PickupComboPoint OnPickupComboPoint;
    public delegate void PickupExperiencePoint();
    public static PickupExperiencePoint OnPickupExperiencePoint;
    public delegate void TouchWeaponLocker();
    public static TouchWeaponLocker OnTouchWeaponLocker;
    public delegate void DamageEnemy(int damage);
    public static DamageEnemy OnDamageEnemy;
    public delegate void DifficultyUp(int level);
    public static DifficultyUp OnDifficultyUp;

    const int COMBO_DAMAGE = 20;
    const int WEAPON_DAMAGE_MULTIPLER = 10;
    const int PLAYER_BASE_DAMAGE = 50;
    const int WEAPON_LEVEL_UP_THRESHOLD = 25;
    const int SCORE_INCREMENT = 1;

    const int LEVEL1_SCORE = 200;
    const int LEVEL2_SCORE = 300;
    const int LEVEL3_SCORE = 600;

    public const float CAMERA_SPEED_LEVEL_0 = 2f;
    public const float CAMERA_SPEED_LEVEL_1 = 2.5f;
    public const float CAMERA_SPEED_LEVEL_2 = 3f;
    public const float CAMERA_SPEED_LEVEL_3 = 4f;

    public const float PARALLAX_FRONT_ROCKS_LEVEL_0 = 0.1f;
    public const float PARALLAX_BACK_ROCKS_LEVEL_0 = 0.1f;
    public const float PARALLAX_MOUNTAINS_LEVEL_0 = 0.7f;
    public const float PARALLAX_BACKGROUND_LEVEL_0 = 0.9f;
    public const float PARALLAX_GROUND_LEVEL_0 = 0f;

    public const float PARALLAX_FRONT_ROCKS_LEVEL_1 = 0.1f;
    public const float PARALLAX_BACK_ROCKS_LEVEL_1 = 0.1f;
    public const float PARALLAX_MOUNTAINS_LEVEL_1 = 0.7f;
    public const float PARALLAX_BACKGROUND_LEVEL_1 = 0.9f;
    public const float PARALLAX_GROUND_LEVEL_1 = 0f;


    [SerializeField] int lives;
    [SerializeField] int dodgePoints = 50;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] HealthBar experienceBar;
    [SerializeField] GameObject levelUpText;

    [HideInInspector] public bool isPlayerAlive;
    public int level;
    public int score;
    public int weaponPoints = 0;

    int enemiesKilled = 0;
    int weaponLevel = 1;
    int weaponExperience = 0;

    Coroutine scoreRoutine;
    AudioManager audioManager;
    WeaponPoints weaponPointsDisplay;
    EnemiesKilled enemiesKilledDisplay;
    WeaponLevel weaponLevelDisplay;
    Score scoreDisplay;

    private void OnEnable()
    {
        OnPickupWeaponPoint += HandlePickupWeaponPoint;
        Enemy.OnDeath += HandleEnemyKilled;
        OnTouchWeaponLocker += HandleWeaponLockerTouch;
        OnPickupComboPoint += HandlePickupComboPoint;
        OnPickupExperiencePoint += HandlePickupExperiencePoint;
    }

    private void OnDisable()
    {
        OnPickupWeaponPoint -= HandlePickupWeaponPoint;
        Enemy.OnDeath -= HandleEnemyKilled;
        OnTouchWeaponLocker -= HandleWeaponLockerTouch;
        OnPickupComboPoint -= HandlePickupComboPoint;
        OnPickupExperiencePoint -= HandlePickupExperiencePoint;
    }

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        weaponPointsDisplay = FindObjectOfType<WeaponPoints>();
        enemiesKilledDisplay = FindObjectOfType<EnemiesKilled>();
        weaponLevelDisplay = FindObjectOfType<WeaponLevel>();
        scoreDisplay = FindObjectOfType<Score>();
    }

    private void Start()
    {
        SetLevel(0);
        SetScore(0);
        ResetWeaponPoints();
        SetEnemiesKilled(enemiesKilled);
        experienceBar.SetMaxHealth(WEAPON_LEVEL_UP_THRESHOLD, false);
        FindObjectOfType<Lives>().Init(lives);

        scoreRoutine = StartCoroutine(ScoreRoutine());
        audioManager.Play("Background_Music");
    }

    public void PlayerDied()
    {
        FindObjectOfType<Enemy>().StopAttacking();
        loseScreen.SetActive(true);
        StopCoroutine(scoreRoutine);
        StartCoroutine(Camera.main.GetComponent<Translate>().Stop());
    }

    public void PlayerHit()
    {
        lives--;
        FindObjectOfType<Lives>().SetHearts(lives);
    }

    public void PlayerDodged()
    {
        SetScore(score + dodgePoints);

        int damage = GetDamage();
        OnDamageEnemy.Invoke(damage);
        FindObjectOfType<Enemy>().GetHit(damage);
    }

    public int GetLives()
    {
        return lives;
    }

    IEnumerator ScoreRoutine()
    {
        while (isPlayerAlive)
        {
            SetScore(score + SCORE_INCREMENT);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public int GetDamage()
    {
        return PLAYER_BASE_DAMAGE * weaponLevel;
    }

    void SpawnEnemy()
    {
        StartCoroutine(DelaySpawn());
    }

    void HandleWeaponLockerTouch()
    {
        Enemy.OnTakeDamage?.Invoke(GetDamage());
        ResetWeaponPoints();
    }

    void HandleEnemyKilled()
    {
        enemiesKilled++;
        SetEnemiesKilled(enemiesKilled);
        SpawnEnemy();
    }

    void HandlePickupWeaponPoint()
    {
        SetWeaponPoints(weaponPoints + 1);
    }

    void HandlePickupComboPoint()
    {
        Enemy enemy = null;
        // enemy stops attacking
        while (!enemy)
        {
            if (FindObjectOfType<Enemy>() != null)
            {
                enemy = FindObjectOfType<Enemy>();
            }
        }
        enemy.StopAttacking();
        StartCoroutine(ComboRoutine());
    }

    void HandlePickupExperiencePoint()
    {
        weaponExperience++;
        if (weaponExperience >= WEAPON_LEVEL_UP_THRESHOLD)
        {
            weaponLevel++;
            weaponExperience = 0;
        }
        experienceBar.SetHealth(weaponExperience);
        weaponLevelDisplay.SetText(weaponLevel);
    }

    void ResetWeaponPoints()
    {
        SetWeaponPoints(0);
    }

    void SetWeaponPoints(int points)
    {
        weaponPoints = points;
        weaponPointsDisplay.SetText(weaponPoints);
    }

    void SetScore(int val)
    {
        score = val;
        scoreDisplay.SetText(score);

        if (score >= LEVEL1_SCORE && level == 0)
        {
            SetLevel(1);
        }

        if (score >= LEVEL2_SCORE && level == 1)
        {
            SetLevel(2);
        }

        if (score >= LEVEL3_SCORE && level == 2)
        {
            SetLevel(3);
        }

        //FindObjectOfType<Enemy>()?.UpdateAnimationSpeeds(); // enemy may be dead
    }

    void SetEnemiesKilled(int val)
    {
        enemiesKilled = val;
        enemiesKilledDisplay.SetText(enemiesKilled);
    }

    IEnumerator ComboRoutine()
    {
        // player does animation
        FindObjectOfType<Player>().ComboAttack();
        // enemy does animation
        // apply damage to enemy
        FindObjectOfType<Enemy>().StopAttacking();
        Enemy.OnTakeDamage?.Invoke(COMBO_DAMAGE);
        // player return to regular state
        yield return new WaitForSeconds(2f);
        FindObjectOfType<Player>().EndComboAttack();
        // update score
        FindObjectOfType<Enemy>()?.StartAttacking(); // enemy may have died from combo
    }

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(enemyPrefab, Camera.main.transform);
    }

    void SetLevel(int val)
    {
        level = val;
        OnDifficultyUp.Invoke(level);
        if (level > 0)
        {
            levelUpText.SetActive(true);
        }
        Debug.Log($"Reached level {level}!");
    }
}
