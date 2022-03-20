using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    const string KEY_JUMPS = "jumps";
    PlayerData loaded;

    private void OnEnable()
    {
        Player.OnJumped += UpdateJumps;
    }

    private void OnDisable()
    {
        Player.OnJumped -= UpdateJumps;
    }

    private void Start()
    {
        LoadFromPrefs();
    }
  
    public void DeleteAllPrefs()
    {
        // UPDATE WHEN KEYS ARE MODIFIED
        List<string> keys = new List<string>();
        keys.Add(KEY_JUMPS);

        foreach (string key in keys)
        {
            PlayerPrefs.DeleteKey(key);
        }

        Debug.Log($"DeleteAllPrefs.");
    }

    void LoadFromPrefs()
    {
        int jumps = PlayerPrefs.GetInt(KEY_JUMPS);

        loaded = new PlayerData(jumps);

        Debug.Log("Init player data..");
        LogLoadedPrefs();
    }

    void UpdateJumps()
    {
        loaded.jumps += 1;
        SaveToPrefs();
    }

    void SaveToPrefs()
    {
        PlayerPrefs.SetInt("jumps", loaded.jumps);

        Debug.Log("SaveToPrefs complete.");
        LogLoadedPrefs();
    }

    void LogLoadedPrefs()
    {
        Debug.Log($"Jumps: {loaded.jumps}");
    }
}
