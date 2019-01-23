using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DTInventory.Models;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Player/PlayerDataModel", order = 1)]
public class PlayerDataModel : SaveDataModel
{
    public Vector3 PlayerLastPosition;
    public Vector2 LastMapPosition;
    public InventoryDataModel InventoryDataModel;
    public override void Init<T>(T saveDataModel)
    {
        var playerDataModel = saveDataModel as PlayerDataModel;
        SavePath = playerDataModel.SavePath;
        PlayerLastPosition = playerDataModel.PlayerLastPosition;
        LastMapPosition = playerDataModel.LastMapPosition;
        InventoryDataModel = playerDataModel.InventoryDataModel;
    }

    public void OnBeforeSave(){
        
    }

    public override ScriptableObject OnLoad()
    {
        var deserializedObj = ScriptableObject.CreateInstance<PlayerDataModel>();
        if (File.Exists(Application.persistentDataPath + SavePath))
        {
            var bf = new BinaryFormatter();
            using (var file = File.Open(Application.persistentDataPath + SavePath, FileMode.Open))
            {
                var deserializedObjString = (System.String)bf.Deserialize(file);
                JsonUtility.FromJsonOverwrite(deserializedObjString, deserializedObj);
            }
        }

        return  deserializedObj;
    }
}
