using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public abstract class SaveDataModel : ScriptableObject, ISavable
{
    [SerializeField]
    private string savePath;
    public string SavePath
    {
        get
        {
            return savePath;
        }

        set
        {
            savePath = value;
        }
    }

    public abstract void Init<T>(T saveDataModel);

    public abstract ScriptableObject OnLoad();

    public void OnSave()
    {        
        var jsonData = JsonUtility.ToJson(this);
        var bf = new BinaryFormatter();
        using (var file = File.Open(Application.persistentDataPath + savePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            bf.Serialize(file, jsonData);
        }
    }
}
