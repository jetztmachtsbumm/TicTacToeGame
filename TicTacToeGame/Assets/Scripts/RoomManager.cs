using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{

    private static RoomManager instance;

    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject playerInTurnInfo;
    [SerializeField] private GameObject waitingForOtherPlayer;

    new private PhotonView photonView;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        photonView = GetComponent<PhotonView>();
    }

    public static RoomManager GetInstance()
    {
        return instance;
    }

    public PhotonView GetPhotonView()
    {
        return photonView;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("StartGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void StartGame()
    {
        Destroy(waitingForOtherPlayer);
        grid.SetActive(true);
        playerInTurnInfo.SetActive(true);
    }

    [PunRPC]
    private void SwitchPlayerInTurn()
    {
        GameManager.GetInstance().SwitchPlayerInTurn();
    }

    [PunRPC]
    private void GameOver()
    {
        GridCell.GameOverMultiplayer();
    }

    [PunRPC]
    private void SyncIconType(int cell, byte iconType)
    {
        GridCell.SetIconTypeOnIndex(cell, iconType);
    }
}
