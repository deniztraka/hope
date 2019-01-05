using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    private float runSpeed = 40f;
    public float RunSpeed;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    void Start()
    {
        runSpeed = RunSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

			var direction = 0f;
			if (touch.position.x < Screen.width / 8f)
            {

                direction = -1f;
            }
            else if (touch.position.x > (Screen.width * (7f / 8f)))
            {
				direction = 1f;
            }

			horizontalMove = direction * runSpeed;
            
        }
        else
        {
            horizontalMove = 0f;
        }


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
