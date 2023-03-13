using Photon.Pun;
using System.IO;
using Assets.Scripts.Player;
using UnityEngine;

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
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint(2);
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
