using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class GameState : MonoBehaviourPun
{
    private PhotonView PV;
    private Dictionary<string, int> Score;
    private Dictionary<string, Player[]> Teams;
    public static GameState Instance { get; private set; }
    public string CurrentScore { get; private set; }
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (Instance == null)
        {
            Debug.Log($"Team Setup");
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
                CurrentScore = string.Join(" | ", Score.Select(_ => $"{_.Key} : {_.Value}"));
            }
            Instance = this;
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
        var team = PhotonNetwork.PlayerList.First(_ => _.NickName == nickName).GetPhotonTeam();
        Debug.Log($"Player team {team?.Name?? "NULL"} ");
        var teamScored = team?.Code == 1 ? Assets.Scripts.Consts.Teams.Team_2.Name : Assets.Scripts.Consts.Teams.Team_1.Name;
        Debug.Log($"Player team {team?.Name ?? "NULL"} score {teamScored}");
        Score[teamScored] += 1;
        var scoreTable = string.Join(" | ", Score.Select(_ => $"{_.Key}:{_.Value}"));
        CurrentScore = scoreTable;
        Debug.Log($"Current Score: {scoreTable}");
    }
}
