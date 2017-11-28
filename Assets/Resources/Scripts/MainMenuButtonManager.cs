using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    public void btn_NewGame(string Javan_Test)
    {
        SceneManager.LoadScene(Javan_Test);
        Debug.Log("loaded");
    }

    public void btn_QuitGame()
    {
        Application.Quit();
    }

    public void btn_Settings(string settingsMenu)
    {
        SceneManager.LoadScene(settingsMenu);
        Debug.Log("LUL");
    }

    public void btn_Credits(string creditsMenu)
    {
        SceneManager.LoadScene(creditsMenu);
        Debug.Log("Kappa");
    }

    public void btn_CreditsBack(string menu)
    {
        SceneManager.LoadScene(menu);
        Debug.Log("hAhAA");
    }

    public void btn_SettingsBack(string menu)
    {
        SceneManager.LoadScene(menu);
        Debug.Log("I'm 12 btw");
    }
}
