using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    [SerializeField] private GameObject crossSprite;
    [SerializeField] private GameObject circleSprite;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject[] UIIcons;
    [SerializeField] private GameObject[] gameOverIcons;
    [SerializeField] private GameObject[] destroyOnGameOver;

    //false = player 1 (Cross), true = player 2 (Circle)
    private bool playerInTurn;

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

    public void SwitchPlayerInTurn()
    {
        playerInTurn = !playerInTurn;
        UIIcons[playerInTurn ? 1 : 0].SetActive(true);
        UIIcons[playerInTurn ? 0 : 1].SetActive(false);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1);

        foreach (GameObject obj in destroyOnGameOver)
        {
            Destroy(obj);
        }
        gameOverScreen.SetActive(true);

        if (!playerInTurn)
        {
            gameOverIcons[0].SetActive(false);
            gameOverIcons[1].SetActive(true);
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
