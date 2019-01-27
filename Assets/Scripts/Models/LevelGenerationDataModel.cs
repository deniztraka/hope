using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelGenerationDataModel : ScriptableObject
{
    public List<ItemGenerationProbabilityDataModel> ItemGenerationProbabilityDataModels;    

    public GameObject LevelStaticObject;
    public float LeftEdge = -108f;
    public float RightEdge = 108f;
}
