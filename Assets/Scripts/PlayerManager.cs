using Photon.Pun;
using System.IO;
using Assets.Scripts.Player;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }

    }

    void CreateController()
    {
        var team = PhotonNetwork.LocalPlayer.GetPhotonTeam();
        Debug.Log($"team {team?.Name ?? "NULL"}"); 
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint((int)team.Code);
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(controller);
    }
}
