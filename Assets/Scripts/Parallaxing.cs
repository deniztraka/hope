using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public float backgrounSize;
    public float parallaxSpeed;
    public bool isScrollingEnabled, isParallaxingEnabled;

    private float lastCameraX;
    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone;
    private int leftIndex;
    private int rightIndex;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;

        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);

            leftIndex = 0;
            rightIndex = layers.Length - 1;
        }
    }

    void Update()
    {
        if (isParallaxingEnabled)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);            
        }

        if (isScrollingEnabled)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
            {
                scrollLeft();
            }

            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
            {
                scrollRight();
            }
        }

        lastCameraX = cameraTransform.position.x;
    }

    void scrollLeft()
    {
        int lastRight = rightIndex;

        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgrounSize);

        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }

    private void scrollRight()
    {
        int lastLeft = leftIndex;

        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgrounSize);

        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }

    // void FixedUpdate()
    // {
    //     if (isParallaxingEnabled)
    //     {
    //         float deltaX = cameraTransform.position.x - lastCameraX;
    //         //transform.position += Vector3.right * (deltaX * parallaxSpeed);
    //         var newPos = transform.position += Vector3.right * (deltaX * parallaxSpeed);
    //         transform.position = Vector3.MoveTowards(transform.position, newPos, parallaxSpeed);
    //     }
    // }
}
