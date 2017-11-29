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
    }

    public void btn_QuitGame()
    {
        Application.Quit();
    }

    public void btn_Settings(string settingsMenu)
    {
        SceneManager.LoadScene(settingsMenu);
    }

    public void btn_Credits(string creditsMenu)
    {
        SceneManager.LoadScene(creditsMenu);
    }

    public void btn_CreditsBack(string menu)
    {
        SceneManager.LoadScene(menu);
    }

    public void btn_SettingsBack(string menu)
    {
        SceneManager.LoadScene(menu);
    }
}
