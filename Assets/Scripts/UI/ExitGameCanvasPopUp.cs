using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameCanvasPopUp : MonoBehaviour
{
    private GameManager gameManager;

    void Awake()
    {
        var gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    public void Yes()
    {
        GameManager.ReturnToMainMenu();
    }

    public void No()
    {
        gameObject.SetActive(false);

    }


}
