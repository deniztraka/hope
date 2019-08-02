using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float Speed = 5f;
    public float RunSpeed;
    float horizontalMove = 0f;
    float lastMoveDirection = 0f;
    bool jump = false;
    bool crouch = false;
    public bool Attacking = false;

    void Start()
    {
        controller.OnCrouchEvent.AddListener(Ping);
    }

    private void Ping(bool arg0)
    {
        Debug.Log(arg0);
    }

    public void SetHorizontalMovement(float direction)
    {
        horizontalMove = direction * Speed;
        if (direction != 0)
        {
            crouch = false;
            lastMoveDirection = direction;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.touchCount > 0)
        // {
        //     var touch = Input.GetTouch(0);

        // 	var direction = 0f;
        // 	if (touch.position.x < Screen.width / 8f)
        //     {

        //         direction = -1f;
        //     }
        //     else if (touch.position.x > (Screen.width * (7f / 8f)))
        //     {
        // 		direction = 1f;
        //     }

        // 	horizontalMove = direction * runSpeed;

        // }
        // else
        // {
        //     horizontalMove = 0f;
        // }


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {            
            crouch = !crouch;
        }
        
        if (Input.GetButtonDown("Attack"))
        {
            Attacking = true;
        }
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        animator.SetFloat("horizontalMovement", horizontalMove);
        animator.SetBool("crouch", crouch);
        animator.SetBool("attacking", Attacking);

        // if (lastMoveDirection != horizontalMove)
        // {
        //     Debug.Log(lastMoveDirection + "," + horizontalMove);
        //     if (horizontalMove != 0)
        //     {
        //         if (lastMoveDirection < 0)
        //         {
        //             animator.Play("WalkWest");
        //         }
        //         else if (lastMoveDirection > 0)
        //         {
        //             animator.Play("WalkEast");
        //         }
        //     }
        //     else
        //     {
        //         if (lastMoveDirection < 0)
        //         {
        //             animator.Play("IdleWest");
        //         }
        //         else if (lastMoveDirection > 0)
        //         {
        //             animator.Play("IdleEast");
        //         }
        //     }
        // }

        return;


    }
}
