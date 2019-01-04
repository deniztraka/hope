using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeaths : MonoBehaviour
{
    public GameObject objectToDrop;
    public int dropCount;

    public void DropItem(Vector3 position)
    {

        var gameObject = Instantiate(objectToDrop, position, Quaternion.Euler(0, 0, 0));
        var rigidBody2d = gameObject.GetComponent<Rigidbody2D>();
        rigidBody2d.AddForce(new Vector2(0f, 10));
    }
}
