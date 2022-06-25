using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GridCell : MonoBehaviour
{

    private static GridCell[] cells = new GridCell[9];
    private static bool gameOver;

    private bool isFilledIn;
    private byte iconType;

    private void Start()
    {
        gameOver = false;
        cells[transform.GetSiblingIndex()] = this;
    }

    public static void GameOverMultiplayer()
    {
        GameManager.GetInstance().StartCoroutine(GameManager.GetInstance().GameOver());
        gameOver = true;
    }

    public static void SetIconTypeOnIndex(int index, byte iconType)
    {
        cells[index].SetIconType(iconType);
    }

    public void OnClick()
    {
        if (isFilledIn || gameOver)
        {
            return;
        }

        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (!(bool)PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"])
                {
                    PhotonNetwork.Instantiate("Cross", transform.position, Quaternion.identity);
                    RoomManager.GetInstance().GetPhotonView().RPC("SwitchPlayerInTurn", RpcTarget.All);
                    SyncIconType(1);
                    CheckWinCondition();
                }
            }
            else
            {
                if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"])
                {
                    PhotonNetwork.Instantiate("Circle", transform.position, Quaternion.identity);
                    RoomManager.GetInstance().GetPhotonView().RPC("SwitchPlayerInTurn", RpcTarget.All);
                    SyncIconType(2);
                    CheckWinCondition();
                }
            }
        }
        else
        {
            if (!GameManager.GetInstance().GetPlayerInTurn())
            {
                //Player 1 (Cross) is in turn
                Instantiate(GameManager.GetInstance().GetCrossSprite(), transform.position, Quaternion.identity, transform);
                iconType = 1;
            }
            else
            {
                //Player 2 (Circle) is in turn
                Instantiate(GameManager.GetInstance().GetCricleSprite(), transform.position, Quaternion.identity, transform);
                iconType = 2;
            }
            CheckWinCondition();
            GameManager.GetInstance().SwitchPlayerInTurn();
        }

        isFilledIn = true;
    }

    private void CheckWinCondition()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (cells[0].iconType == 1 && cells[1].iconType == 1 && cells[2].iconType == 1
                || cells[3].iconType == 1 && cells[4].iconType == 1 && cells[5].iconType == 1
                || cells[6].iconType == 1 && cells[7].iconType == 1 && cells[8].iconType == 1
                || cells[0].iconType == 1 && cells[3].iconType == 1 && cells[6].iconType == 1
                || cells[1].iconType == 1 && cells[4].iconType == 1 && cells[7].iconType == 1
                || cells[2].iconType == 1 && cells[5].iconType == 1 && cells[8].iconType == 1
                || cells[0].iconType == 1 && cells[4].iconType == 1 && cells[8].iconType == 1
                || cells[2].iconType == 1 && cells[4].iconType == 1 && cells[6].iconType == 1)
                {
                    RoomManager.GetInstance().GetPhotonView().RPC("GameOver", RpcTarget.All);
                }
            }
            else
            {
                if (cells[0].iconType == 2 && cells[1].iconType == 2 && cells[2].iconType == 2
                || cells[3].iconType == 2 && cells[4].iconType == 2 && cells[5].iconType == 2
                || cells[6].iconType == 2 && cells[7].iconType == 2 && cells[8].iconType == 2
                || cells[0].iconType == 2 && cells[3].iconType == 2 && cells[6].iconType == 2
                || cells[1].iconType == 2 && cells[4].iconType == 2 && cells[7].iconType == 2
                || cells[2].iconType == 2 && cells[5].iconType == 2 && cells[8].iconType == 2
                || cells[0].iconType == 2 && cells[4].iconType == 2 && cells[8].iconType == 2
                || cells[2].iconType == 2 && cells[4].iconType == 2 && cells[6].iconType == 2)
                {
                    RoomManager.GetInstance().GetPhotonView().RPC("GameOver", RpcTarget.All);
                }
            }
        }
        else
        {
            if (!GameManager.GetInstance().GetPlayerInTurn())
            {                        
                if(cells[0].iconType == 1 && cells[1].iconType == 1 && cells[2].iconType == 1
                || cells[3].iconType == 1 && cells[4].iconType == 1 && cells[5].iconType == 1
                || cells[6].iconType == 1 && cells[7].iconType == 1 && cells[8].iconType == 1
                || cells[0].iconType == 1 && cells[3].iconType == 1 && cells[6].iconType == 1
                || cells[1].iconType == 1 && cells[4].iconType == 1 && cells[7].iconType == 1
                || cells[2].iconType == 1 && cells[5].iconType == 1 && cells[8].iconType == 1
                || cells[0].iconType == 1 && cells[4].iconType == 1 && cells[8].iconType == 1
                || cells[2].iconType == 1 && cells[4].iconType == 1 && cells[6].iconType == 1)
                {
                    StartCoroutine(GameManager.GetInstance().GameOver());
                    gameOver = true;
                }
            }
            else
            {
                if (cells[0].iconType == 2 && cells[1].iconType == 2 && cells[2].iconType == 2
                ||  cells[3].iconType == 2 && cells[4].iconType == 2 && cells[5].iconType == 2
                ||  cells[6].iconType == 2 && cells[7].iconType == 2 && cells[8].iconType == 2
                ||  cells[0].iconType == 2 && cells[3].iconType == 2 && cells[6].iconType == 2
                ||  cells[1].iconType == 2 && cells[4].iconType == 2 && cells[7].iconType == 2
                ||  cells[2].iconType == 2 && cells[5].iconType == 2 && cells[8].iconType == 2
                ||  cells[0].iconType == 2 && cells[4].iconType == 2 && cells[8].iconType == 2
                ||  cells[2].iconType == 2 && cells[4].iconType == 2 && cells[6].iconType == 2)
                {
                    StartCoroutine(GameManager.GetInstance().GameOver());
                    gameOver = true;
                }
            }
        }
    }

    private void SyncIconType(byte iconType)
    {
        int indexOfThisCell = 0;
        for(int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == this)
            {
                indexOfThisCell = i;
                break;
            }
        }

        RoomManager.GetInstance().GetPhotonView().RPC("SyncIconType", RpcTarget.All, indexOfThisCell, iconType);
    }

    private void SetIconType(byte iconType)
    {
        this.iconType = iconType;
    }

}
