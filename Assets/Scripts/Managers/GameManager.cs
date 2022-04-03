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
    public delegate void DamageBoss(int damage);
    public static DamageBoss OnDamageBoss;

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
    [SerializeField] Transform playerBossPosition;

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
        // must register every enemy type
        Enemy1.OnDeath += HandleEnemyKilled;
        Boss.OnDeath += HandleBossKilled;
        ///////////////////////////////////
        Player.OnHit += PlayerHit;
        Player.OnDeath += PlayerDied;
        Player.OnDodged += PlayerDodged;
        OnTouchWeaponLocker += HandleWeaponLockerTouch;
    }

    private void OnDisable()
    {
        OnPickupWeaponPoint -= HandlePickupWeaponPoint;
        Enemy1.OnDeath -= HandleEnemyKilled;
        Boss.OnDeath -= HandleBossKilled;
        Player.OnHit -= PlayerHit;
        Player.OnDeath -= PlayerDied;
        Player.OnDodged -= PlayerDodged;
        OnTouchWeaponLocker -= HandleWeaponLockerTouch;
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
        // NOTE: enemy and boss events are both invoked when dodge occurs.
        bool isBossAlive = FindObjectOfType<Boss>() != null;
        if (isBossAlive)
        {
            OnDamageBoss?.Invoke(GetDamage());
        }
        else
        {
            OnDamageEnemy?.Invoke(GetDamage());
        }
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
        bool isDamagingBoss = FindObjectOfType<Boss>() != null;
        if (isDamagingBoss)
        {
            return (int)(PLAYER_BASE_DAMAGE * weaponLevel * FindObjectOfType<Boss>().DAMAGE_REDUCTION);
        }
        return PLAYER_BASE_DAMAGE * weaponLevel;
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

        if (weaponLevel >= 3)
        {
            Vector3 newPos = FindObjectOfType<Player>().transform.position;
            newPos.x = playerBossPosition.position.x;
            FindObjectOfType<Player>().transform.position = newPos;
            Enemy1[] enemyRefs = FindObjectsOfType<Enemy1>();
            foreach (Enemy1 enemyRef in enemyRefs)
            {
                Destroy(enemyRef.gameObject);
            }
            FindObjectOfType<EnemySpawner>().StopSpawning();
            FindObjectOfType<EnemySpawner>().SpawnBoss();
        }
    }

    void HandleBossKilled()
    {

    }

    void HandlePickupWeaponPoint()
    {
        SetWeaponPoints(weaponPoints + 1);
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
