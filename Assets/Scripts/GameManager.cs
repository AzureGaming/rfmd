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

    const int COMBO_DAMAGE = 20;
    const int WEAPON_DAMAGE_MULTIPLER = 10;
    const int PLAYER_BASE_DAMAGE = 30;
    const int WEAPON_LEVEL_UP_THRESHOLD = 50;

    [HideInInspector] public bool isPlayerAlive;

    [SerializeField] int lives;
    [SerializeField] int dodgePoints = 50;
    [SerializeField] int scoreIncrement;
    [SerializeField] float level2Threshold = 15f;
    [SerializeField] float level3Threshold = 30f;
    [SerializeField] float level4Threshold = 45f;
    [SerializeField] float level5Threshold = 58.5f;
    [SerializeField] float level6Threshold = 72f;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] HealthBar experienceBar;

    public int level = 1;
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
    DodgeTimingManager dodgeTimingManager;

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
        dodgeTimingManager = FindObjectOfType<DodgeTimingManager>();
        weaponLevelDisplay = FindObjectOfType<WeaponLevel>();
    }

    private void Start()
    {
        ResetWeaponPoints();
        experienceBar.SetMaxHealth(WEAPON_LEVEL_UP_THRESHOLD, false);
        enemiesKilled = 0;
        FindObjectOfType<Lives>().Init(lives);
        scoreRoutine = StartCoroutine(ScoreRoutine());
        StartCoroutine(LevelUpRoutine());
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
        score += dodgePoints;

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
            score += scoreIncrement;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator LevelUpRoutine()
    {
        float timeElapsed = 0f;
        while (isPlayerAlive)
        {
            timeElapsed += Time.deltaTime;
            if (level == 1 && timeElapsed >= level2Threshold
                || level == 2 && timeElapsed >= level3Threshold
                || level == 3 && timeElapsed >= level4Threshold
                || level == 4 && timeElapsed >= level5Threshold
                || level == 5 && timeElapsed >= level6Threshold)
            {
                level++;
                FindObjectOfType<Enemy>()?.UpdateAnimationSpeeds();
                Debug.Log($"Reached level {level}!");
            }

            yield return null;
        }
    }

    public int GetDamage()
    {
        //Debug.Log("calculate damage" + dodgeTimingManager.level);
        //switch(dodgeTimingManager.level) {
        //    case DodgeTimingManager.DodgeLevel.LEVEL_1:
        //        return 10;
        //    case DodgeTimingManager.DodgeLevel.LEVEL_2:
        //        return 20;
        //    case DodgeTimingManager.DodgeLevel.LEVEL_3:
        //        return 30;
        //    default:
        //        return 10;
        //}
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
}
