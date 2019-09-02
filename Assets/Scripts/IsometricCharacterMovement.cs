using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCharacterMovement : MonoBehaviour
{
    public FixedJoystick joystick;

    public bool isRunning = false;
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
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");        
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        //inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed * (isRunning ? 1.5f : 1f);
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        characterAnimatorHandler.SetDirection(movement, isRunning);
        rbody.MovePosition(newPos);

        //Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        //rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

    }
}
