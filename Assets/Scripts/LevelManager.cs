using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform levelPart1;
    [SerializeField] Transform levelPart2;
    [SerializeField] Transform levelPart3;
    [SerializeField] Transform horizontalGroupHigh;
    [SerializeField] Transform levelPartStart;
    [SerializeField] Transform levelPartCheckpoint;

    const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 10f;
    const float PLAYER_DISTANCE_SPAWN_CHECKPOINT = 20f;
    const int STARTING_LEVEL_PARTS = 5;
    float levelPartOffset = 5f;

    Player player;
    Vector3 lastEndPos;
    Vector3 lastCheckpointPos;
    List<Transform> levelPartList;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        levelPartList = new List<Transform>();
        levelPartList.Add(levelPart1);
        levelPartList.Add(levelPart2);
        levelPartList.Add(levelPart3);
    }

    private void Start()
    {
        lastEndPos = levelPartStart.position;
        for (int i = 0; i < STARTING_LEVEL_PARTS - 1; i++)
        {
            SpawnLevelPart();
        }

        SpawnCheckpoint(new Vector3(player.transform.position.x + PLAYER_DISTANCE_SPAWN_CHECKPOINT, 0f));
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, lastEndPos) < PLAYER_DISTANCE_SPAWN_LEVEL_PART)
        {
            SpawnLevelPart();
        }

        if (Vector3.Distance(player.transform.position, lastCheckpointPos) < PLAYER_DISTANCE_SPAWN_CHECKPOINT)
        {
            SpawnCheckpoint();
        }
    }

    void SpawnLevelPart()
    {
        Vector3 endPos = lastEndPos;
        endPos.x += levelPartOffset;

        Transform levelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        Transform lastLevelPart = SpawnLevelPart(levelPart, endPos);
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
