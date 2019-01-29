using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DTInventory.MonoBehaviours;
using DTInventory.Models;
using System;
using DTComponents;

public class Player : MonoBehaviour
{

    public delegate void PlayerEventHandler();
    public event PlayerEventHandler OnFirstUpdate;
    public PlayerDataModel PlayerDataModel;
    public InventoryBehavior InventoryBehaviour;

    public int Damage = 20;

    private bool isFirstUpdatePast;

    void Awake()
    {

    }

    void Start()
    {
        EventManager.StartListening("OnBeforeSave", OnBeforeSave);
        LoadValues();
    }

    void Update()
    {

        if (!isFirstUpdatePast)
        {
            isFirstUpdatePast = true;
            if (OnFirstUpdate != null)
            {
                OnFirstUpdate();
            }
        }
    }

    public void LoadValues()
    {
        transform.position = PlayerDataModel.PlayerLastPosition;
    }

    internal int GetCurrentDamage()
    {
        return Damage;
    }

    public void OnBeforeSave()
    {
        PlayerDataModel.PlayerLastPosition = transform.position;
        InventoryBehaviour.UpdateDataModel();
        PlayerDataModel.InventoryDataModel = InventoryBehaviour.InventoryDataModel;
        PlayerDataModel.Health = GetComponent<Health>().CurrentValue;
        PlayerDataModel.Toughness = GetComponent<Toughness>().CurrentValue;
        PlayerDataModel.Energy = GetComponent<Energy>().CurrentValue;
    }

    public bool PickUpItem(Item item)
    {
        return InventoryBehaviour.Add(item);
    }

}
