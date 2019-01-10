using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    void Start()
    {
        var playerDataModel = GameManager.Instance.PlayerDataModel;
        var levelDataModel = GameManager.Instance.Savables.Find(obj => obj.GetType() == typeof(LevelDataModel) && obj.name == string.Format("Level_{0}-{1}", playerDataModel.LastMapPosition.x, playerDataModel.LastMapPosition.y)) as LevelDataModel;
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

        }

        // if (MapPositionX < 0 || MapPositionY < 0 || !IsVisitedBefore)
        // {
        //     //this is new map lets generate it


        //     if (levelGenerationDataModel == null)
        //     {
        //         return;
        //     }
        //     CreateSideScrollMap(levelGenerationDataModel);
        //     SaveSideScrollMap();
        // }
        // else
        // {
        //     LoadSideScrollMap(levelDataModel, levelGenerationDataModel);

        // }
    }

    void Update()
    {

    }

    private void SaveSideScrollMap()
    {
        var playerGameObject = GameObject.Find("Player");
        var player = playerGameObject.GetComponent<Player>();
        var lastMapPosition = player.PlayerDataModel.LastMapPosition;

        var levelDataModel = GameManager.Instance.Savables.Find(obj => obj.GetType() == typeof(LevelDataModel) && obj.name == string.Format("Level_{0}-{1}", lastMapPosition.x, lastMapPosition.y)) as LevelDataModel;
        var objectsContainer = GameObject.Find("ObjectsContainer");
        levelDataModel.GeneratedObjects = new List<GeneratedItemDataModel>();


        foreach (Transform child in objectsContainer.transform)
        {
            levelDataModel.GeneratedObjects.Add(
                new GeneratedItemDataModel()
                {
                    Prefab = child.gameObject.name.Replace("(Clone)", string.Empty),
                    Position = child.position
                }
            );
        }
        levelDataModel.Position = player.PlayerDataModel.LastMapPosition;
        levelDataModel.IsVisitedBefore = true;

    }

    private LevelGenerationDataModel GetLevelGenerationDataModel(SideScrollMapType sideScrollMapType)
    {
        return LevelGenerationDataModels.Find(model => model.name.Equals(sideScrollMapType.ToString()));
    }

    private void LoadSideScrollMap(LevelDataModel levelDataModel, LevelGenerationDataModel levelGenerationDataModel)
    {
        var mapStaticObject = CreateStatics(levelGenerationDataModel);
        var objectsContainer = GameObject.Find("ObjectsContainer");
        LoadMapContent(levelDataModel, objectsContainer);
    }

    private void LoadMapContent(LevelDataModel levelDataModel, GameObject container)
    {
        foreach (var generatedObject in levelDataModel.GeneratedObjects)
        {   
            
            var prefab = Resources.Load("Prefabs/" + generatedObject.Prefab, typeof(GameObject));
            if (prefab != null)
            {
                Instantiate(prefab, generatedObject.Position, Quaternion.identity);
            }
        }
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
        for (int i = 0; i < levelGenerationDataModel.IntensityLevel; i++)
        {
            //loop trough each item in generation content
            foreach (var itemGenerationDataModel in levelGenerationDataModel.ItemGenerationProbabilityDataModels)
            {
                var probability = itemGenerationDataModel.Probability;

                //check luck to instantiate object in the map
                if (Random.Range(0, levelGenerationDataModel.IntensityLevel) < probability)
                {
                    //get random variant
                    var variantIndex = Random.Range(0, itemGenerationDataModel.GameObjectVariants.Count);
                    var gameObjectVariant = itemGenerationDataModel.GameObjectVariants[variantIndex];

                    //get random x position withing the map size
                    var randomX = Random.Range(levelGenerationDataModel.LeftEdge, levelGenerationDataModel.RightEdge);


                    //instantiate game object in random x position
                    var instantiatedObject = Instantiate(itemGenerationDataModel.GameObjectVariants[variantIndex],
                        new Vector3(randomX, gameObjectVariant.transform.position.y, gameObjectVariant.transform.position.z),
                        Quaternion.identity, container.transform);
                }
            }
        }
    }

    //floor and background creation
    private GameObject CreateStatics(LevelGenerationDataModel levelGenerationDataModel)
    {
        return Instantiate(levelGenerationDataModel.LevelStaticObject, Vector3.zero, Quaternion.identity, transform);
    }
}
