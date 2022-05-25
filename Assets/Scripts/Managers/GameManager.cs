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
    public delegate void CompleteRun(int currencyThisRun);
    public static CompleteRun OnCompleteRun;

    const int COMBO_DAMAGE = 20;
    const int WEAPON_DAMAGE_MULTIPLER = 10;
    const int PLAYER_BASE_DAMAGE = 999;
    const int WEAPON_LEVEL_UP_THRESHOLD = 25;
    const int SCORE_INCREMENT = 1;
    const int MAX_LIVES = 5;

    const int LEVEL1_SCORE = 200;
    const int LEVEL2_SCORE = 400;
    const int LEVEL3_SCORE = 600;
    const int LEVEL4_SCORE = 800;

    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject resultsScreen;
    [SerializeField] GameObject gameScreen;
    [SerializeField] GameObject mainMenu;
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
        weaponLevelDisplay = FindObjectOfType<WeaponLevel>();
        scoreDisplay = FindObjectOfType<Score>();
        SetLevel(0);
    }

    private void Start()
    {
        //SetGameObjectActive(loseScreen, false);
        //SetGameObjectActive(resultsScreen, false);
        //SetGameObjectActive(gameScreen, false);
        //SetGameObjectActive(mainMenu, true);
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

    public void StartRun()
    {
        SetGameObjectActive(loseScreen, false);
        SetGameObjectActive(resultsScreen, false);
        SetGameObjectActive(gameScreen, false);
        SetGameObjectActive(mainMenu, false);
        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        yield return StartCoroutine(FindObjectOfType<RunSetup>().IntroAnimation());

        SetGameObjectActive(gameScreen, true);
        timeDisplay = FindObjectOfType<TimeElapsed>();
        enemiesKilledDisplay = FindObjectOfType<EnemiesKilled>();

        SetLevel(1);
        SetScore(0);
        SetEnemiesKilled(enemiesKilled);
        FindObjectOfType<Lives>()?.Init(MAX_LIVES);
        FindObjectOfType<PhaseManager>().InitPhases();

        scoreRoutine = StartCoroutine(ScoreRoutine());
        audioManager?.Play("Background_Music");
    }

    void SetGameObjectActive(GameObject obj, bool val)
    {
        if (obj != null)
        {
            obj.SetActive(val);
        }
    }

    void UpdateTime()
    {
        minutes = (int)Time.timeSinceLevelLoad / 60;
        seconds = (int)Time.timeSinceLevelLoad % 60;

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
        SetWeaponLevel(1);
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
        StopCoroutine(scoreRoutine);
        enemiesKilled++;
        SetEnemiesKilled(enemiesKilled);
        runCurrency += 50;
        OnCompleteRun?.Invoke(runCurrency);
        resultsScreen.SetActive(true);
        gameScreen.SetActive(false);
    }

    void SetWeaponLevel(int value)
    {
        weaponLevel = value;
        //weaponLevelDisplay.SetText(weaponLevel);
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
