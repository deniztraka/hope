﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCharacterMovement : MonoBehaviour
{
    public float movementSpeed = 1f;
    public CharacterAnimatorHandler characterAnimatorHandler;

    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");        
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        //inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        characterAnimatorHandler.SetDirection(movement);
        rbody.MovePosition(newPos);
    }
}
