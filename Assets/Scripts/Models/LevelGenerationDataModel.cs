using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelGenerationDataModel : ScriptableObject
{
    public List<ItemGenerationProbabilityDataModel> ItemGenerationProbabilityDataModels;

    public GameObject LevelStaticObject;

    public int FullyGenerationTimeInSeconds = 600;

    public float LeftEdge = -108f;
    public float RightEdge = 108f;
}
