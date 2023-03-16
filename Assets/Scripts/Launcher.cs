using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;
using Assets.Scripts.Extensions;
using Assets.Scripts.Consts;
using System;

public class Launcher : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] Transform playerListTeamOne;
    [SerializeField] Transform playerListTeamTwo;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject teamOneButton;
    [SerializeField] GameObject teamTwoButton;

    private Dictionary<int, Transform> playerLists;

    private byte JOIN_TEAM_EVENTTYPE = 1;

    void Awake()
    {
        Instance = this;
        playerLists = new Dictionary<int, Transform> { { 0, playerListContent }, { 1, playerListTeamOne }, { 2, playerListTeamTwo } };
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            var team = player.GetPhotonTeam()?.Code ?? 0;
            Instantiate(PlayerListItemPrefab, playerLists[team]).GetComponent<PlayerListItem>().SetUp(player);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void ChangeTeam(int team)
    {
        (team == 1 ? teamOneButton : teamTwoButton).SetActive(false);
        (team != 1 ? teamOneButton : teamTwoButton).SetActive(true);
        Player player = PhotonNetwork.LocalPlayer;
        var content = new object[] { player.NickName, team };
        PhotonNetwork.RaiseEvent(JOIN_TEAM_EVENTTYPE, content, PhotonRaiseEvent.ToAll, SendOptions.SendReliable);
    }

    private void ChangeTeamPlayer(string nickName, int team)
    {
        var player = PhotonNetwork.PlayerList.First(_ => _.NickName == nickName);
        var currentTeam = player.GetPhotonTeam()?.Code ?? 0;
        if (currentTeam == team) return;
        player.JoinTeam(team);
        foreach (Transform child in playerLists[currentTeam])
        {
            var listItem = child.GetComponent<PlayerListItem>();
            if (listItem.player.NickName != player.NickName) continue;
            Destroy(child.gameObject);
            Instantiate(PlayerListItemPrefab, playerLists[team]).GetComponent<PlayerListItem>().SetUp(player);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        ResetAll();
        PhotonNetwork.LeaveRoom();
        foreach (int key in playerLists.Keys)
            foreach (Transform child in playerLists[key])
            {
                Destroy(child.gameObject);
            }
        MenuManager.Instance.OpenMenu("loading");
    }

    private void ResetAll()
    {
        teamOneButton.SetActive(true);
        teamTwoButton.SetActive(true);
        if (PhotonNetwork.LocalPlayer.GetPhotonTeam() != null)
            PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomItemList>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == JOIN_TEAM_EVENTTYPE)
        {
            object[] data = (object[])photonEvent.CustomData;

            var nickName = (string)data[0];
            var team = (int)data[1];

            ChangeTeamPlayer(nickName, team);
        }
    }
}
