using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public Button NewGameButton;
    public Button ContinueButton;


    void Start()
    {
        if (Directory.Exists(GameManager.SavePath))
        {
            ContinueButton.interactable = true;
        }
        else
        {
            ContinueButton.interactable = false;
        }
    }

    public void NewGame()
    {
        var gameObj = GameObject.Find("GameManager");
        var gameManager = gameObj.GetComponent<GameManager>();
        gameManager.SetFreshStart();       
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        GameManager.LoadGame();
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
