using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] Spawnpoint[] spawnpointsTeamOne;
    [SerializeField] Spawnpoint[] spawnpointsTeamTwo;

    void Awake()
    {
        Instance = this;
    }

    public Transform GetSpawnPoint(int team)
    {
        Debug.Log($"GetSpawnPoint: team {team}");
        if (team == 1)
        {
            return spawnpointsTeamOne[Random.Range(0, spawnpointsTeamOne.Length)].transform;

        }
        else { 
            return spawnpointsTeamTwo[Random.Range(0, spawnpointsTeamTwo.Length)].transform;

        }
    }
}
