using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform levelPart1;
    [SerializeField] Transform levelPart2;
    [SerializeField] Transform levelPart3;
    [SerializeField] Transform levelPart4;
    [SerializeField] Transform levelPart5;
    [SerializeField] Transform levelPart6;
    [SerializeField] Transform levelPart7;
    [SerializeField] Transform horizontalGroupHigh;
    [SerializeField] Transform levelPartStart;
    [SerializeField] Transform levelPartCheckpoint;

    const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 8f; // must be updated if camera speeds up
    const float PLAYER_DISTANCE_SPAWN_CHECKPOINT = 20f;
    const float GROUND_Y = -0.45f;
    const int STARTING_LEVEL_PARTS = 3;
    float levelPartOffset = 8f;

    Player player;
    Vector3 lastEndPos;
    Vector3 lastCheckpointPos;
    List<Transform> levelPartList;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        levelPartList = new List<Transform>();
        //levelPartList.Add(levelPart1);
        //levelPartList.Add(levelPart2);
        //levelPartList.Add(levelPart3);
        //levelPartList.Add(levelPart4);
        //levelPartList.Add(levelPart5);
        //levelPartList.Add(levelPart6);
        levelPartList.Add(levelPart7);
    }

    private void Start()
    {
        lastEndPos = levelPartStart.position;
        for (int i = 0; i < STARTING_LEVEL_PARTS - 1; i++)
        {
            SpawnLevelPart();
        }

        //SpawnCheckpoint(new Vector3(player.transform.position.x + PLAYER_DISTANCE_SPAWN_CHECKPOINT, 0f));
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, lastEndPos) < PLAYER_DISTANCE_SPAWN_LEVEL_PART)
        {
            SpawnLevelPart();
        }

        //if (Vector3.Distance(player.transform.position, lastCheckpointPos) < PLAYER_DISTANCE_SPAWN_CHECKPOINT)
        //{
        //    SpawnCheckpoint();
        //}
    }

    void SpawnLevelPart()
    {
        Vector3 levelPartPos = lastEndPos;
        levelPartPos.x += levelPartOffset;
        levelPartPos.y = GROUND_Y;

        Transform levelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        Transform lastLevelPart = SpawnLevelPart(levelPart, levelPartPos);
        lastEndPos = lastLevelPart.Find("EndPosition").position;
    }

    void SpawnCheckpoint()
    {
        Transform lastCheckpoint = Instantiate(levelPartCheckpoint, lastCheckpointPos, Quaternion.identity);
        lastCheckpointPos = lastCheckpoint.Find("EndPosition").position;
    }

    void SpawnCheckpoint(Vector3 pos)
    {
        Transform lastCheckpoint = Instantiate(levelPartCheckpoint, pos, Quaternion.identity);
        lastCheckpointPos = lastCheckpoint.position;
    }

    Transform SpawnLevelPart(Transform part, Vector3 pos)
    {
        return Instantiate(part, pos, Quaternion.identity);
    }
}
