using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    public GameObject player;       //Public variable to store a reference to the player game object
    public float height;
    public float distance;
    public float damping;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //var wantedPosition = player.transform.TransformPoint(0, height, -distance);
        //var wantedPosition = player.transform.position + offset;
        var wantedPosition = new Vector3(player.transform.position.x, 1.350019f, -3.4f);
        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

    }
}
