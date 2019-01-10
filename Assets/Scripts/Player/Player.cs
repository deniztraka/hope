using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public PlayerDataModel PlayerDataModel;

    void Awake()
    {
        LoadValues();
    }

    void Start(){
        EventManager.StartListening ("OnBeforeSave", OnBeforeSave);
    }

    public void OnBeforeSave()
    {
        PlayerDataModel.PlayerLastPosition = transform.position;
    }

    public void LoadValues(){
        transform.position = PlayerDataModel.PlayerLastPosition;
    }

}
