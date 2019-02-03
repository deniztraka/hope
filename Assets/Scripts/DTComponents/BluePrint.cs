using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrint : MonoBehaviour
{
    public GameObject StaticObject;

    public void OnUse()
    {
        var objectsContainer = GameObject.Find("SideScrollMap").transform.Find("ObjectsContainer");
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        Instantiate(StaticObject, new Vector3(playerObj.transform.position.x, 1.85f, playerObj.transform.position.z), Quaternion.identity, objectsContainer.transform);
    }
}
