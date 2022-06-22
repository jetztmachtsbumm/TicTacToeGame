using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    public void OnOnlineClicked()
    {

    }

    public void OnLocalClicked()
    {
        SceneManager.LoadScene("LocalPlay");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

}
