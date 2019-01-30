using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DTEngines;
using DTInventory.Models;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Player/PlayerDataModel", order = 1)]
public class PlayerDataModel : SaveDataModel
{
    public float Health;
    public float Toughness;
    public float Energy;
    public Vector3 PlayerLastPosition;
    public Vector2 LastMapPosition;
    public InventoryDataModel InventoryDataModel;
    public long RealGameSecondsPast;

    public override void Init<T>(T saveDataModel)
    {
        var playerDataModel = saveDataModel as PlayerDataModel;
        SavePath = playerDataModel.SavePath;
        PlayerLastPosition = playerDataModel.PlayerLastPosition;
        LastMapPosition = playerDataModel.LastMapPosition;
        InventoryDataModel = playerDataModel.InventoryDataModel;
        Health = playerDataModel.Health;
        Toughness = playerDataModel.Toughness;
        Energy = playerDataModel.Energy;
        RealGameSecondsPast = playerDataModel.RealGameSecondsPast;
    }

    public void OnBeforeSave()
    {

    }

    public override ScriptableObject OnLoad()
    {
        var path = Application.persistentDataPath + SavePath;
        Debug.Log(path + " | " + this.name);
        var deserializedObj = ScriptableObject.CreateInstance<PlayerDataModel>();
        if (File.Exists(path))
        {
            var bf = new BinaryFormatter();
            using (var file = File.Open(path, FileMode.Open))
            {
                var deserializedObjString = (System.String)bf.Deserialize(file);
                JsonUtility.FromJsonOverwrite(deserializedObjString, deserializedObj);
            }
        }

        return deserializedObj;
    }
}
