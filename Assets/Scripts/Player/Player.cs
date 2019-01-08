using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public PlayerDataModel PlayerDataModel;

    void Awake()
    {
        transform.position = PlayerDataModel.PlayerLastPosition;
    }

    void Start(){
        EventManager.StartListening ("OnBeforeSave", OnBeforeSave);
    }

    public void OnBeforeSave()
    {
        PlayerDataModel.PlayerLastPosition = transform.position;
    }

}
