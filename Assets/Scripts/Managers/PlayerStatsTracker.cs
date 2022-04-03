using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    const string KEY_JUMPS = "jumps";
    const string KEY_SUCCESSFUL_DODGES = "successful_dodges";
    const string KEY_DAMAGE_DONE = "damage_done";
    const string KEY_ENEMIES_KILLED = "enemies_killed";

    PlayerData loaded;

    private void OnEnable()
    {
        Player.OnJumped += UpdateJumps;
        Player.OnDodged += UpdateSuccesfulDodges;
        GameManager.OnDamageEnemy += UpdateDamageDone;
        Enemy1.OnDeath += UpdateEnemiesKilled;
    }

    private void OnDisable()
    {
        Player.OnJumped -= UpdateJumps;
        Player.OnDodged -= UpdateSuccesfulDodges;
        GameManager.OnDamageEnemy -= UpdateDamageDone;
        Enemy1.OnDeath -= UpdateEnemiesKilled;
    }

    private void Start()
    {
        LoadFromPrefs();
    }

    public void DeleteAllPrefs()
    {
        // UPDATE WHEN KEYS ARE MODIFIED //
        PlayerPrefs.DeleteKey(KEY_JUMPS);
        PlayerPrefs.DeleteKey(KEY_SUCCESSFUL_DODGES);
        PlayerPrefs.DeleteKey(KEY_DAMAGE_DONE);
        PlayerPrefs.DeleteKey(KEY_ENEMIES_KILLED);
        ///////////////////////////////////

        Debug.Log($"DeleteAllPrefs.");
    }

    void LoadFromPrefs()
    {
        // UPDATE WHEN KEYS ARE MODIFIED //
        int jumps = PlayerPrefs.GetInt(KEY_JUMPS);
        int successfulDodges = PlayerPrefs.GetInt(KEY_SUCCESSFUL_DODGES);
        int damageDone = PlayerPrefs.GetInt(KEY_DAMAGE_DONE);
        int enemiesKilled = PlayerPrefs.GetInt(KEY_ENEMIES_KILLED);
        ///////////////////////////////////

        loaded = new PlayerData(jumps, successfulDodges, damageDone, enemiesKilled);

        Debug.Log("Init player data..");
        LogLoadedPrefs();
    }

    void UpdateJumps()
    {
        loaded.jumps += 1;
        SaveToPrefs();
    }

    void UpdateSuccesfulDodges()
    {
        loaded.successfulDodges += 1;
        SaveToPrefs();
    }

    void UpdateDamageDone(int damage)
    {
        loaded.damageDone += damage;
        SaveToPrefs();
    }

    void UpdateEnemiesKilled()
    {
        loaded.enemiesKilled += 1;
        SaveToPrefs();
    }

    void SaveToPrefs()
    {
        // UPDATE WHEN KEYS ARE MODIFIED //
        PlayerPrefs.SetInt(KEY_JUMPS, loaded.jumps);
        PlayerPrefs.SetInt(KEY_SUCCESSFUL_DODGES, loaded.successfulDodges);
        PlayerPrefs.SetInt(KEY_DAMAGE_DONE, loaded.damageDone);
        PlayerPrefs.SetInt(KEY_ENEMIES_KILLED, loaded.enemiesKilled);
        ///////////////////////////////////

        LogLoadedPrefs();
    }

    void LogLoadedPrefs()
    {
        Debug.Log($"Jumps: {loaded.jumps} - Successful Dodges: {loaded.successfulDodges} - Damage Done: {loaded.damageDone} - Enemies Killed: {loaded.enemiesKilled}");
    }
}
