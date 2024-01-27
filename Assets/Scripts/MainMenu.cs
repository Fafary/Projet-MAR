using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayMap1()
    {
        SceneManager.LoadScene("Niveau2");
    }

    public void PlayMap2()
    {
        SceneManager.LoadScene("Niveau1");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
