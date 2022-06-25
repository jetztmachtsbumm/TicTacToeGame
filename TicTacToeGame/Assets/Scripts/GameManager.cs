using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    [SerializeField] private GameObject crossSprite;
    [SerializeField] private GameObject circleSprite;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject[] UIIcons;
    [SerializeField] private GameObject[] playerIconInfo;
    [SerializeField] private GameObject[] gameOverIcons;
    [SerializeField] private GameObject[] destroyOnGameOver;

    private PhotonView[] players = new PhotonView[2];

    //false = player 1 (Cross), true = player 2 (Circle)
    private bool playerInTurn;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"] = false;
                if (!PhotonNetwork.IsMasterClient)
                {
                    playerIconInfo[0].SetActive(false);
                    playerIconInfo[1].SetActive(true);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckMouseClick();
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public bool GetPlayerInTurn()
    {
        if (PhotonNetwork.InRoom)
        {
            return (bool) PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"];
        }
        return playerInTurn;
    }

    public GameObject GetCrossSprite()
    {
        return crossSprite;
    }

    public GameObject GetCricleSprite()
    {
        return circleSprite;
    }

    public PhotonView[] GetPlayers()
    {
        return players;
    }

    public void SwitchPlayerInTurn()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"] = !(bool)PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"];
            UIIcons[(bool) PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"] ? 1 : 0].SetActive(true);
            UIIcons[(bool) PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"] ? 0 : 1].SetActive(false);
        }
        else
        {
            playerInTurn = !playerInTurn;
            UIIcons[playerInTurn ? 1 : 0].SetActive(true);
            UIIcons[playerInTurn ? 0 : 1].SetActive(false);
        }
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("DestroyOnGameOver"))
        {
            Destroy(obj);
        }
        foreach (GameObject obj in destroyOnGameOver)
        {
            obj.SetActive(false);
        }
        gameOverScreen.SetActive(true);

        if (PhotonNetwork.InRoom)
        {
            if (!(bool)PhotonNetwork.CurrentRoom.CustomProperties["playerInTurn"])
            {
                gameOverIcons[0].SetActive(false);
                gameOverIcons[1].SetActive(true);
            }
        }
        else
        {
            if (!playerInTurn)
            {
                gameOverIcons[0].SetActive(false);
                gameOverIcons[1].SetActive(true);
            }
        }
    }

    public void OnPlayAgainClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.transform.CompareTag("GridCell"))
                {
                    hit.transform.GetComponent<GridCell>().OnClick();
                }
            }
        }
    }


}
