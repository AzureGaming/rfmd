using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void DifficultyUp(int level);
    public static DifficultyUp OnDifficultyUp;
    public delegate void DamageEnemy(int damage);
    public static DamageEnemy OnDamageEnemy;
    public delegate void DamagePlayer();
    public static DamagePlayer OnDamagePlayer;
    public delegate void DamageBoss(int damage);
    public static DamageBoss OnDamageBoss;

    const int COMBO_DAMAGE = 20;
    const int WEAPON_DAMAGE_MULTIPLER = 10;
    const int PLAYER_BASE_DAMAGE = 25;
    const int WEAPON_LEVEL_UP_THRESHOLD = 25;
    const int SCORE_INCREMENT = 1;

    const int LEVEL1_SCORE = 200;
    const int LEVEL2_SCORE = 400;
    const int LEVEL3_SCORE = 600;
    const int LEVEL4_SCORE = 800;

    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject resultsScreen;
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject levelUpText;
    [SerializeField] Transform playerBossPosition;

    public int lives;
    public int dodgePoints = 50;
    public bool isPlayerAlive;
    public int level;
    public int score;
    public int weaponPoints = 0;
    public int minutes = 0;
    public int seconds = 0;
    [Header("Dev Settings")] public bool showCooldowns;
    public int enemiesKilled { get; private set; } = 0;
    public int runCurrency { get; private set; } = 0;

    int weaponLevel = 1;
    int weaponExperience = 0;

    Coroutine scoreRoutine;
    AudioManager audioManager;
    EnemiesKilled enemiesKilledDisplay;
    WeaponLevel weaponLevelDisplay;
    Score scoreDisplay;
    TimeElapsed timeDisplay;

    private void OnEnable()
    {
        Enemy.OnDeath += HandleEnemyKilled;
        Boss.OnDeath += HandleBossKilled;
        Player.OnHit += PlayerHit;
        Player.OnDeath += PlayerDied;
        Player.OnDodged += PlayerDodged;
    }

    private void OnDisable()
    {
        Enemy.OnDeath -= HandleEnemyKilled;
        Boss.OnDeath -= HandleBossKilled;
        Player.OnHit -= PlayerHit;
        Player.OnDeath -= PlayerDied;
        Player.OnDodged -= PlayerDodged;
    }

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        enemiesKilledDisplay = FindObjectOfType<EnemiesKilled>();
        weaponLevelDisplay = FindObjectOfType<WeaponLevel>();
        scoreDisplay = FindObjectOfType<Score>();
        timeDisplay = FindObjectOfType<TimeElapsed>();
        SetLevel(0); // enemy needs this before running start
    }

    private void Start()
    {
        loseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        mainScreen.SetActive(true);

        SetScore(0);
        SetEnemiesKilled(enemiesKilled);
        FindObjectOfType<Lives>()?.Init(lives);

        scoreRoutine = StartCoroutine(ScoreRoutine());
        audioManager?.Play("Background_Music");
    }

    private void Update()
    {
        if (timeDisplay == null)
        {
            return;
        }
        ShowCooldownText();
        UpdateTime();
    }

    void UpdateTime()
    {
        minutes = (int)Time.time / 60;
        seconds = (int)Time.time % 60;

        timeDisplay.SetText(minutes, seconds);
    }

    void ShowCooldownText()
    {
        CooldownText[] cooldownTexts = FindObjectsOfType<CooldownText>(true);
        if (showCooldowns)
        {
            foreach (CooldownText text in cooldownTexts)
            {
                text.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (CooldownText text in cooldownTexts)
            {
                text.gameObject.SetActive(false);
            }
        }
    }

    void PlayerDied()
    {
        loseScreen.SetActive(true);
        StopCoroutine(scoreRoutine);
    }

    void PlayerHit()
    {
        lives--;
        SetWeaponLevel(0);
        FindObjectOfType<Lives>().SetHearts(lives);
        OnDamagePlayer?.Invoke();
    }

    void PlayerDodged(Enemy enemyRef)
    {
        SetScore(score + dodgePoints);
        int damage = GetDamage();
        enemyRef.TakeDamage(damage);
        OnDamageEnemy?.Invoke(damage);
        SetWeaponLevel(weaponLevel + 1);
    }

    IEnumerator ScoreRoutine()
    {
        while (isPlayerAlive)
        {
            SetScore(score + SCORE_INCREMENT);
            yield return new WaitForSeconds(0.5f);
        }
    }

    int GetDamage()
    {
        // Note: manual check of boss
        bool isDamagingBoss = FindObjectOfType<Boss>() != null;
        if (isDamagingBoss)
        {
            return (int)(PLAYER_BASE_DAMAGE * weaponLevel * FindObjectOfType<Boss>().DAMAGE_REDUCTION);
        }
        return PLAYER_BASE_DAMAGE * weaponLevel;
    }

    void HandleEnemyKilled(Enemy _)
    {
        runCurrency += 5;
        enemiesKilled++;
        SetEnemiesKilled(enemiesKilled);
    }

    void HandleBossKilled()
    {
        resultsScreen.SetActive(true);
        StopCoroutine(scoreRoutine);
    }

    void SetWeaponLevel(int value)
    {
        weaponLevel = value;
        weaponLevelDisplay.SetText(weaponLevel);
    }

    void SetScore(int val)
    {
        score = val;
        scoreDisplay?.SetText(score);
        //UpdateLevel();
    }

    void UpdateLevel()
    {
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
        if (score >= LEVEL4_SCORE && level == 3)
        {
            SetLevel(4);
        }
    }

    void SetEnemiesKilled(int val)
    {
        enemiesKilled = val;
        enemiesKilledDisplay?.SetText(enemiesKilled);
    }

    void SetLevel(int val) // should be trash enemy centric
    {
        level = val;
        OnDifficultyUp.Invoke(level);
        if (level > 0 && levelUpText != null)
        {
            levelUpText.SetActive(true);
        }
    }
}
