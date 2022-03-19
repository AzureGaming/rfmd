using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void PickupWeaponPoint();
    public static PickupWeaponPoint OnPickupWeaponPoint;
    public delegate void EnemyKilled();
    public static EnemyKilled OnEnemyKilled;
    public delegate void TouchWeaponLocker();
    public static TouchWeaponLocker OnTouchWeaponLocker;


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

    public int level = 1;
    public int score;
    public int weaponPoints = 0;

    int enemiesKilled = 0;

    Coroutine scoreRoutine;
    AudioManager audioManager;
    WeaponPoints weaponPointsDisplay;
    EnemiesKilled enemiesKilledDisplay;

    private void OnEnable()
    {
        OnPickupWeaponPoint += HandlePickupWeaponPoint;
        OnEnemyKilled += HandleEnemyKilled;
        OnTouchWeaponLocker += HandleWeaponLockerTouch;
    }

    private void OnDisable()
    {
        OnPickupWeaponPoint -= HandlePickupWeaponPoint;
        OnEnemyKilled -= HandleEnemyKilled;
        OnTouchWeaponLocker -= HandleWeaponLockerTouch;
    }

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        weaponPointsDisplay = FindObjectOfType<WeaponPoints>();
        enemiesKilledDisplay = FindObjectOfType<EnemiesKilled>();
    }

    private void Start()
    {
        ResetWeaponPoints();
        enemiesKilled = 0;
        FindObjectOfType<Lives>().Init(lives);
        scoreRoutine = StartCoroutine(ScoreRoutine());
        StartCoroutine(LevelUpRoutine());
        audioManager.Play("Background_Music");
    }

    public void PlayerDied()
    {
        FindObjectOfType<Enemy>().Stop();
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
                FindObjectOfType<Enemy>().UpdateAnimationSpeeds();
                Debug.Log($"Reached level {level}!");
            }

            yield return null;
        }
    }

    public int GetDamage()
    {
        return weaponPoints * 10;
    }

    void SpawnEnemy()
    {
        Debug.Log("Spawn Enenemy");
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

    IEnumerator DelaySpawn()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(enemyPrefab, Camera.main.transform);
    }
}
