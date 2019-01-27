using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "SideScrollMap/LevelDataModel", order = 1)]
public class LevelDataModel : SaveDataModel
{
    public Vector2 Position;

    public SideScrollMapType LevelType;

    public Boolean IsVisitedBefore;

    public List<GeneratedItemDataModel> GeneratedObjects;
    public override void Init<T>(T saveDataModel)
    {
        var levelDataModel = saveDataModel as LevelDataModel;
        SavePath = levelDataModel.SavePath;
        Position = levelDataModel.Position;
        LevelType = levelDataModel.LevelType;
        GeneratedObjects = levelDataModel.GeneratedObjects;
        IsVisitedBefore = levelDataModel.IsVisitedBefore;
    }

    public override ScriptableObject OnLoad()
    {
        var path = Application.persistentDataPath + SavePath;
        Debug.Log(path + " | " + this.name);
        var deserializedObj = ScriptableObject.CreateInstance<LevelDataModel>();
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
