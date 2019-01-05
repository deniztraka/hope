using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeData : MonoBehaviour
{
    private LevelData levelData;
    private int numberOfTreeCount;

    public List<GameObject> treeTypes;
    public int maxTreeCount = 10;
    public int minTreeCount = 5;
    public int NumberOfTreeCount
    {
        get { return numberOfTreeCount; }
    }
    public List<GameObject> treeList = new List<GameObject>();

    void Start()
    {
        levelData = gameObject.GetComponentInParent<LevelData>();

        GenerateTrees();
    }

    private void GenerateTrees(){
        numberOfTreeCount = Random.Range(minTreeCount, maxTreeCount);
        for (int i = 0; i < numberOfTreeCount; i++)
        {
            var randomTreeTypeIndex = Random.Range(0, treeTypes.Count);
            var randomXPosition = Random.Range(levelData.leftEdge, levelData.rightEdge);

            treeList.Add(Instantiate(treeTypes[randomTreeTypeIndex], new Vector3(randomXPosition, 1.35f, -1), Quaternion.identity));
        }
    }
}
