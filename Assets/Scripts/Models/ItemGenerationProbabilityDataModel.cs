using System;
using System.Collections;
using System.Collections.Generic;
using DTObjects.Statics;
using UnityEngine;

[Serializable]
public class ItemGenerationProbabilityDataModel
{
    public List<GameObject> GameObjectVariants;
    public GameObjectType Type;
    public float Probability;

    public float RegenerateProbabilty;

    public int Intensity;
}
