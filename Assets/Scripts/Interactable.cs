using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float distance = 1.5f;


    public bool IsCloseEnough()
    {
        var player = GameObject.FindWithTag("Player");        
        return Math.Abs(player.transform.position.x - transform.position.x) < distance;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(distance, distance));
    }
}
