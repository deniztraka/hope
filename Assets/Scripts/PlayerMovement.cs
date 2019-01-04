using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    // Update is called once per frame
    void Update()
    {

		//horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            //Debug.Log(touch.position.x);
            if (touch.position.x < Screen.width / 8)
            {

                horizontalMove = -1f * runSpeed;
            }
            else if (touch.position.x > Screen.width * (7 / 8))
            {
                horizontalMove = 1f * runSpeed;
            }
        }
        else
        {
            horizontalMove = 0f;
        }
#endif



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
