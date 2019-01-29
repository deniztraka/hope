using System.Collections;
using System.Collections.Generic;
using DTObjects.Statics;
using UnityEngine;

public class GameStaticObjectSpawner : MonoBehaviour
{
    private SideScrollMap sideScrollMap;
    private bool lockFlag;
    public bool IsEnabled;
    private LevelDataModel levelDataModel;
    private LevelGenerationDataModel levelGenerationDataModel;
    private GameObject objectsContainer;

    public int EngineFrequency;

    void Start()
    {
        IsEnabled = true;

        sideScrollMap = GetComponent<SideScrollMap>();
        if (sideScrollMap == null)
        {
            IsEnabled = false;
            // Debug.Log("ItemSpawner: Item Spawner is not working because it could not reach the SideScrollMap.");
            return;
        }

        levelDataModel = sideScrollMap.GetLevelDataModel();
        levelGenerationDataModel = sideScrollMap.GetLevelGenerationDataModel(levelDataModel.LevelType);
        objectsContainer = GameObject.Find("ObjectsContainer");
    }

    void FixedUpdate()
    {
        if (!lockFlag && IsEnabled)
        {
            lockFlag = true;
            StartCoroutine(Process());
        }
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(EngineFrequency);
        ProcessGeneration();
    }

    public void ProcessGeneration()
    {

        // Debug.Log("-- Starting GameStaticObject spawning process.");

        // Debug.Log("-- Getting already generated static item list.");
        //Get already generated Items
        var alreadyGeneratedItems = new List<GeneratedItemDataModel>();
        foreach (Transform child in objectsContainer.transform)
        {
            var gameStaticObject = child.GetComponent<GameStaticObject>();
            alreadyGeneratedItems.Add(
                new GeneratedItemDataModel()
                {
                    Prefab = child.gameObject.name.Replace("(Clone)", string.Empty),
                    Position = child.position,
                    Type = gameStaticObject.Type
                }
            );
        }

        // Debug.Log("-- Already generated objects count: " + alreadyGeneratedItems.Count);

        // Debug.Log("-- Iterating probModels");

        //Iterate every prob model
        foreach (var probModel in levelGenerationDataModel.ItemGenerationProbabilityDataModels)
        {
            // Debug.Log("-- -- Iterating probModel: " + probModel.BasePrefabName.ToString());

            var alreadyGeneratedItemsForThisModel = alreadyGeneratedItems.FindAll(obj => obj.Prefab.StartsWith(probModel.BasePrefabName));
            var deltaCount = probModel.Intensity - alreadyGeneratedItemsForThisModel.Count;

            // Debug.Log("-- -- Already generated items count for this model is: " + alreadyGeneratedItemsForThisModel.Count);
            // Debug.Log("-- -- Intensity delta count for this model is: " + deltaCount);

            //deltacount > 0 means we are under intensity level and we can try to instantiate item.
            if (deltaCount > 0)
            {
                var gameObject = TryCreateItem(probModel);
                // if (gameObject != null)
                // {
                //     Debug.Log("-- -- GameObject is created for this probModel: " + probModel.BasePrefabName);
                // }
                // else
                // {
                //     Debug.Log("-- -- GameObject is not created for this probModel: " + probModel.BasePrefabName);
                // }
            }
        }

        // Debug.Log("-- GameStaticObject spawning process is finished.");

        lockFlag = false;
    }

    private GameObject TryCreateItem(ItemGenerationProbabilityDataModel probModel)
    {
        // Debug.Log("-- -- -- Trying the create item for this model: " + probModel.Type.ToString());
        return sideScrollMap.TryInstantiateGameObject(probModel, levelGenerationDataModel, objectsContainer, true);
    }
}
