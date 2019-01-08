using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SideScrollMap : MonoBehaviour
{
    private const string Format = "Data/SideScrollMapTypes/{0}";
    public SideScrollMapType SideScrollMapType;
    public List<LevelGenerationDataModel> LevelGenerationDataModels;
    public int MapPositionX = -1;
    public int MapPositionY = -1;
    public bool IsVisitedBefore = false;

    void Start()
    {

        if (MapPositionX < 0 || MapPositionY < 0 || !IsVisitedBefore)
        {
            //this is new map lets generate it

            var levelGenerationDataModel = GetLevelGenerationDataModel(SideScrollMapType);
            if (levelGenerationDataModel == null)
            {
                return;
            }
            CreateSideScrollMap(levelGenerationDataModel);
            SaveSideScrollMap();
        }
        else
        {
            //this map is already created lets build it again from save folder

        }
    }

    void Update()
    {

    }

    private void SaveSideScrollMap()
    {
        //TODO: Save SideScrollMap
    }

    private LevelGenerationDataModel GetLevelGenerationDataModel(SideScrollMapType sideScrollMapType)
    {
        return LevelGenerationDataModels.Find(model => model.name.Equals(sideScrollMapType.ToString()));
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
