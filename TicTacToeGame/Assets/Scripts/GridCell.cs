using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void OnClick()
    {
        if (isFilledIn || gameOver)
        {
            return;
        }

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
        isFilledIn = true;
    }

    private void CheckWinCondition()
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
