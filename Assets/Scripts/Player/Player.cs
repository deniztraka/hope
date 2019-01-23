using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DTInventory.MonoBehaviours;
using DTInventory.Models;

public class Player : MonoBehaviour
{
    public PlayerDataModel PlayerDataModel;
    public InventoryBehavior InventoryBehaviour;

    void Awake()
    {
        LoadValues();
    }

    void Start()
    {
        EventManager.StartListening("OnBeforeSave", OnBeforeSave);        
    }

    public void LoadValues()
    {
        transform.position = PlayerDataModel.PlayerLastPosition;        
        
    }

    public void OnBeforeSave()
    {
        PlayerDataModel.PlayerLastPosition = transform.position;
        InventoryBehaviour.UpdateDataModel();
        PlayerDataModel.InventoryDataModel = InventoryBehaviour.InventoryDataModel;
    }

    public bool PickUpItem(Item item){        
        return InventoryBehaviour.Add(item);
    }

}
