using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private SideScrollMap sideScrollMap;
    void Start(){
        sideScrollMap = GetComponent<SideScrollMap>();

        if(sideScrollMap == null){
            Debug.Log("ItemSpawner: Item Spawner is not working because it could not reach the SideScrollMap.");
        }
    }
}
