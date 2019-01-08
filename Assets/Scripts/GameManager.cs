﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    //TODO: Event Sistem ile OnBeforeSave sırasında gerekli scriptableobject datalarını set et. 
    //Örneğin player last position datası
    //Player içinde OnBeforeSave e register ol ve PlayerDataModel'i update et.
    //Böylece loading sırasında sıkıntı çıkmaz çünkü ana menüye geçerken dosyayı sisteme kaydediyor.

    private static GameManager _instance;


    public static GameManager Instance { get { return _instance; } }

    public List<SaveDataModel> Savables;
    public PlayerDataModel PlayerDataModel;

    public static string SavePath
    {
        get
        {
            return Application.persistentDataPath + "\\Save";
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    internal void SetFreshStart()
    {
        if (Directory.Exists(SavePath))
        {
            Directory.Delete(SavePath, true);
        }
        var directory = Directory.CreateDirectory(SavePath);
        directory.Create();
        directory.CreateSubdirectory("Player");

        PlayerDataModel.PlayerLastPosition = new Vector3(0f, -0.2183512f, -3f);
    }

    void OnBeforeSave()
    {

    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            var results = Resources.FindObjectsOfTypeAll<ExitGameCanvasPopUp>();
            var exitGameCanvas = results[0].gameObject;
            exitGameCanvas.SetActive(true);
        }
    }

    public static void ReturnToMainMenu()
    {
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.name.Equals("GameScene"))
        {
            EventManager.TriggerEvent("OnBeforeSave");
            SaveGame();
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    public static void SaveGame()
    {
        Instance.Savables.ForEach(savableObject =>
        {
            savableObject.OnSave();
        });
    }

    public static void LoadGame()
    {
        Instance.Savables.ForEach(savableObject =>
        {
            switch (savableObject.GetType().ToString())
            {
                case "PlayerDataModel":
                    savableObject.Init(savableObject.OnLoad());
                    break;
            }
        });
    }
}
