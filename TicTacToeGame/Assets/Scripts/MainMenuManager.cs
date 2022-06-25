using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuManager : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject connectingText;
    [SerializeField] private GameObject onlineButton;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnOnlineClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLocalClicked()
    {
        SceneManager.LoadScene("LocalPlay");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Destroy(connectingText);
        onlineButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        int roomName = Random.Range(0, 5000);

        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2,
            PublishUserId = true
        };

        PhotonNetwork.CreateRoom("Room_" + roomName, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }

}
