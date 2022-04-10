using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void StartLevel1()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

