using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    string SavePath
    {
        get; set;
    }
    void OnSave();
    ScriptableObject OnLoad();
}
