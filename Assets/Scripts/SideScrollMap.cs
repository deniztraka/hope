using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DTObjects.Statics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SideScrollMap : MonoBehaviour
{
    private const string Format = "Data/SideScrollMapTypes/{0}";
    public SideScrollMapType SideScrollMapType;
    public List<LevelGenerationDataModel> LevelGenerationDataModels;
    public List<LevelDataModel> LevelDataModel;

    public int MapPositionX = -1;
    public int MapPositionY = -1;
    public bool IsVisitedBefore = false;

    private LevelDataModel levelDataModel;

    void Start()
    {
        var playerDataModel = GameManager.Instance.PlayerDataModel;
        levelDataModel = GameManager.Instance.Savables.Find(obj => obj.GetType() == typeof(LevelDataModel) && obj.name == string.Format("Level_{0}-{1}", playerDataModel.LastMapPosition.x, playerDataModel.LastMapPosition.y)) as LevelDataModel;
        var levelGenerationDataModel = GetLevelGenerationDataModel(levelDataModel.LevelType);

        if (!levelDataModel.IsVisitedBefore)
        {
            //this is new map lets generate it
            if (levelGenerationDataModel == null)
            {
                return;
            }
            CreateSideScrollMap(levelGenerationDataModel);
            SaveSideScrollMap();
        }
        else
        {
            LoadSideScrollMap(levelDataModel, levelGenerationDataModel);
            SaveSideScrollMap();
        }



        EventManager.StartListening("OnBeforeSave", OnBeforeSave);

    }

    private void OnBeforeSave()
    {
        SaveSideScrollMap();
    }

    void Update()
    {

    }

    private void SaveSideScrollMap()
    {
        var playerGameObject = GameObject.Find("Player");
        var player = playerGameObject.GetComponent<Player>();
        var lastMapPosition = player.PlayerDataModel.LastMapPosition;

        levelDataModel = GameManager.Instance.Savables.Find(obj => obj.GetType() == typeof(LevelDataModel) && obj.name == string.Format("Level_{0}-{1}", lastMapPosition.x, lastMapPosition.y)) as LevelDataModel;
        var objectsContainer = GameObject.Find("ObjectsContainer");
        levelDataModel.GeneratedObjects = new List<GeneratedItemDataModel>();

        foreach (Transform child in objectsContainer.transform)
        {
            var gameStaticObject = child.GetComponent<GameStaticObject>();
            levelDataModel.GeneratedObjects.Add(
                new GeneratedItemDataModel()
                {
                    Prefab = child.gameObject.name.Replace("(Clone)", string.Empty),
                    Position = child.position,
                    Type = gameStaticObject.Type
                }
            );
        }
        levelDataModel.Position = player.PlayerDataModel.LastMapPosition;
        levelDataModel.IsVisitedBefore = true;
        levelDataModel.LastVisitTime = DateTime.Now.Ticks;

        //Debug.Log("generated objects count " + levelDataModel.GeneratedObjects.Count);
    }

    public LevelGenerationDataModel GetLevelGenerationDataModel(SideScrollMapType sideScrollMapType)
    {
        return LevelGenerationDataModels.Find(model => model.name.Equals(sideScrollMapType.ToString()));
    }

    public LevelDataModel GetLevelDataModel()
    {
        return levelDataModel;
    }

    private void LoadSideScrollMap(LevelDataModel levelDataModel, LevelGenerationDataModel levelGenerationDataModel)
    {
        var mapStaticObject = CreateStatics(levelGenerationDataModel);
        var objectsContainer = GameObject.Find("ObjectsContainer");

        // In here we are give a chance to createing harvested or destroyed game objects again.
        foreach (var probabilityDataModel in levelGenerationDataModel.ItemGenerationProbabilityDataModels)
        {
            var alreadyGeneratedItemsForThisModel = levelDataModel.GeneratedObjects.FindAll(obj => obj.Prefab.StartsWith(probabilityDataModel.BasePrefabName));
            var deltaCount = probabilityDataModel.Intensity - alreadyGeneratedItemsForThisModel.Count;
            if(deltaCount <= 0){
                deltaCount = 0;
            }

            //calculating new delta count according to lastVisitTime
            var currentTime = DateTime.Now;
            var timePastSinceLastVisit = new TimeSpan(currentTime.Ticks - levelDataModel.LastVisitTime);
            var newDeltaCount = Math.Floor(((float)(timePastSinceLastVisit.TotalSeconds) / (float)levelGenerationDataModel.FullyGenerationTimeInSeconds ) * deltaCount);
            if(newDeltaCount >= deltaCount){
                newDeltaCount = deltaCount;
            }
            Debug.Log("deltacount " + deltaCount);
            Debug.Log("newdeltacount " + newDeltaCount);
            // trying to create game objects per amount of delta count
            var counter = 0;
            for (int i = 0; i < newDeltaCount; i++)
            {
                var gameObject = TryInstantiateGameObject(probabilityDataModel, levelGenerationDataModel, objectsContainer, false);
                if(gameObject != null){
                    counter++;
                }
            }

            Debug.Log(counter + " " + probabilityDataModel.BasePrefabName + " is created since last visit.");

        }


        // crateing game objects those are already there
        foreach (var generatedObject in levelDataModel.GeneratedObjects)
        {

            var prefab = Resources.Load("Prefabs/" + generatedObject.Prefab, typeof(GameObject));
            if (prefab != null)
            {
                Instantiate(prefab, generatedObject.Position, Quaternion.identity, objectsContainer.transform);
            }
        }
    }

    public GameObject TryInstantiateGameObject(ItemGenerationProbabilityDataModel probabilityDataModel, LevelGenerationDataModel levelGenerationDataModel, GameObject container, bool isDeltaRegen = false)
    {
        GameObject instantiatedObject = null;
        var rnd = UnityEngine.Random.Range(0f, 1f);
        var probability = isDeltaRegen ? probabilityDataModel.RegenerateProbabilty : probabilityDataModel.Probability;
        //check luck to instantiate object in the map
        if (rnd < probability)
        {
            //get random variant
            var variantIndex = UnityEngine.Random.Range(0, probabilityDataModel.GameObjectVariants.Count);
            var gameObjectVariant = probabilityDataModel.GameObjectVariants[variantIndex];

            //get random x position withing the map size
            var randomX = UnityEngine.Random.Range(levelGenerationDataModel.LeftEdge, levelGenerationDataModel.RightEdge);


            //instantiate game object in random x position
            instantiatedObject = Instantiate(probabilityDataModel.GameObjectVariants[variantIndex],
                new Vector3(randomX, gameObjectVariant.transform.position.y, gameObjectVariant.transform.position.z),
                Quaternion.identity, container.transform);

            var gameStaticObject = instantiatedObject.GetComponent<GameStaticObject>();
            gameStaticObject.Type = probabilityDataModel.Type;
        }

        return instantiatedObject;
    }

    private void CreateSideScrollMap(LevelGenerationDataModel levelGenerationDataModel)
    {
        var mapStaticObject = CreateStatics(levelGenerationDataModel);
        var objectsContainer = GameObject.Find("ObjectsContainer");
        GenerateMapContent(levelGenerationDataModel, objectsContainer);
    }

    private void GenerateMapContent(LevelGenerationDataModel levelGenerationDataModel, GameObject container)
    {
        //try to create items according to intensity level
        // for (int i = 0; i < levelGenerationDataModel.IntensityLevel; i++)
        // {
        //loop trough each item in generation content
        foreach (var probabilityDataModel in levelGenerationDataModel.ItemGenerationProbabilityDataModels)
        {
            //try to create items according to its intensity level
            for (int i = 0; i < probabilityDataModel.Intensity; i++)
            {
                //tryin to create game objects
                TryInstantiateGameObject(probabilityDataModel, levelGenerationDataModel, container);
            }

        }
        // }
    }

    //floor and background creation
    private GameObject CreateStatics(LevelGenerationDataModel levelGenerationDataModel)
    {
        return Instantiate(levelGenerationDataModel.LevelStaticObject, Vector3.zero, Quaternion.identity, transform);
    }
}
