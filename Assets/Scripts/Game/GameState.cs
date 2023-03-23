using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "GDE/Game/State", fileName = "GameState")]
public class GameState : MonoBehaviourPun
{
    private PhotonView PV;
    private Dictionary<string, int> Score;
    private Dictionary<string, Player[]> Teams;
    public static GameState Instance { get; private set; }
    public GameState()
    {
        Instance = this;
    }

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        Score = new Dictionary<string, int>();
        Teams = new Dictionary<string, Player[]>();
        foreach (var team in PhotonTeamsManager.Instance.GetAvailableTeams())
        {
            Score.Add(team.Name, 0);
            Debug.Log($"Team {team.Name} added!");
            if (PhotonTeamsManager.Instance.TryGetTeamMembers(team, out var players))
            {
                Teams.Add(team.Name, players);
                foreach (var player in players)
                {
                    Debug.Log($"player {player.NickName} added | Team {team.Name}!");
                }
            }
        }
    }
    public void OnPlayerDied(string playerNickName)
    {
        PV.RPC("RPC_PlayerDied", RpcTarget.All, playerNickName);
    }
    [PunRPC]
    void RPC_PlayerDied(string nickName)
    {
        Debug.Log($"Player {nickName} died!");
        var scoreTable = string.Join(" | ", Score.Select(_ => $"{_.Key}:{_.Value}"));
        Debug.Log($"Current Score: {scoreTable}");
    }
}
