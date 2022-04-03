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
    public delegate void DifficultyUp(int level);
    public static DifficultyUp OnDifficultyUp;
    public delegate void DamageEnemy(int damage);
    public static DamageEnemy OnDamageEnemy;

    const int COMBO_DAMAGE = 20;
    const int WEAPON_DAMAGE_MULTIPLER = 10;
    const int PLAYER_BASE_DAMAGE = 50;
    const int WEAPON_LEVEL_UP_THRESHOLD = 25;
    const int SCORE_INCREMENT = 1;

    const int LEVEL1_SCORE = 200;
    const int LEVEL2_SCORE = 300;
    const int LEVEL3_SCORE = 600;

    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] HealthBar experienceBar;
    [SerializeField] GameObject levelUpText;

    public int lives;
    public int dodgePoints = 50;
    public bool isPlayerAlive;
    public int level;
    public int score;
    public int weaponPoints = 0;
    public bool isFirstEnemy = true;

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
        Enemy1.OnDeath += HandleEnemyKilled;
        Player.OnHit += PlayerHit;
        Player.OnDeath += PlayerDied;
        Player.OnDodged += PlayerDodged;
        OnTouchWeaponLocker += HandleWeaponLockerTouch;
        OnPickupComboPoint += HandlePickupComboPoint;
        OnPickupExperiencePoint += HandlePickupExperiencePoint;
    }

    private void OnDisable()
    {
        OnPickupWeaponPoint -= HandlePickupWeaponPoint;
        Enemy1.OnDeath -= HandleEnemyKilled;
        Player.OnHit -= PlayerHit;
        Player.OnDeath -= PlayerDied;
        Player.OnDodged -= PlayerDodged;
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
        SetLevel(0); // enemy needs this before running start
    }

    private void Start()
    {
        SetScore(0);
        ResetWeaponPoints();
        SetEnemiesKilled(enemiesKilled);
        experienceBar?.SetMaxHealth(WEAPON_LEVEL_UP_THRESHOLD, false);
        FindObjectOfType<Lives>()?.Init(lives);

        scoreRoutine = StartCoroutine(ScoreRoutine());
        audioManager?.Play("Background_Music");
    }

    void PlayerDied()
    {
        loseScreen.SetActive(true);
        StopCoroutine(scoreRoutine);
    }

    void PlayerHit()
    {
        lives--;
        FindObjectOfType<Lives>().SetHearts(lives);
    }

    void PlayerDodged()
    {
        SetScore(score + dodgePoints);
        OnDamageEnemy?.Invoke(GetDamage());
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
        OnDamageEnemy?.Invoke(GetDamage());
        ResetWeaponPoints();
    }

    void HandleEnemyKilled()
    {
        if (isFirstEnemy)
        {
            isFirstEnemy = false;
        }
        enemiesKilled++;
        SetEnemiesKilled(enemiesKilled);


        SetWeaponExp(weaponExperience + 10);

        //if (enemiesKilled == 2)
        //{
        //    FindObjectOfType<EnemySpawner>().StopSpawning();
        //    FindObjectOfType<EnemySpawner>().SpawnBoss();
        //}
        //SpawnEnemy();
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
        //enemy.StopAttacking();
        StartCoroutine(ComboRoutine());
    }

    void HandlePickupExperiencePoint()
    {
        SetWeaponExp(weaponExperience + 1);
    }

    void ResetWeaponPoints()
    {
        SetWeaponPoints(0);
    }

    void SetWeaponExp(int value)
    {
        weaponExperience += value;
        if (weaponExperience >= WEAPON_LEVEL_UP_THRESHOLD)
        {
            weaponLevel++;
            weaponExperience = 0;
        }
        experienceBar.SetHealth(weaponExperience);
        weaponLevelDisplay.SetText(weaponLevel);

    }

    void SetWeaponPoints(int points)
    {
        weaponPoints = points;
        weaponPointsDisplay?.SetText(weaponPoints);
    }

    void SetScore(int val)
    {
        score = val;
        scoreDisplay?.SetText(score);

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
        enemiesKilledDisplay?.SetText(enemiesKilled);
    }

    IEnumerator ComboRoutine()
    {
        // player does animation
        FindObjectOfType<Player>().ComboAttack();
        // enemy does animation
        // apply damage to enemy
        //FindObjectOfType<Enemy>().StopAttacking();
        OnDamageEnemy?.Invoke(COMBO_DAMAGE);
        // player return to regular state
        yield return new WaitForSeconds(2f);
        FindObjectOfType<Player>().EndComboAttack();
        // update score
        //FindObjectOfType<Enemy>()?.StartAttacking(); // enemy may have died from combo
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
        if (level > 0 && levelUpText != null)
        {
            levelUpText.SetActive(true);
        }
        Debug.Log($"Reached level {level}!");
    }
}
