using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    public GameObject objectToDrop;
    public int dropCount;

    public void DropItem(Vector3 position)
    {

        Instantiate(objectToDrop, position, Quaternion.Euler(0, 0, 0));
    }
}
