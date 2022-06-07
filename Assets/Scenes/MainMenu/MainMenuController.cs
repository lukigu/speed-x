using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string startGameLevelName;
    [SerializeField] private string workshopLevelName;
    [SerializeField] private string settingsLevelName;


    public void startGame() {
        SceneManager.LoadScene(this.startGameLevelName);
    }

    public void startSplitScreene()
    {
        SceneManager.LoadScene("SplitScreenScene");
    }

    public void openWorkshop()
    {
        SceneManager.LoadScene(this.workshopLevelName);
    }

    public void openSettings() {
        SceneManager.LoadScene(this.settingsLevelName);
    }

    public void exit() {
        Application.Quit();
    }
}
