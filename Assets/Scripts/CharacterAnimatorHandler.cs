using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorHandler : MonoBehaviour
{
    public static readonly string[] staticDirections = { "idle_north", "idle_north_west", "idle_west", "idle_south_west", "idle_south", "idle_south_east", "idle_east", "idle_north_east" };    

    public static readonly string[] walkingDirections = { "walking_north", "walking_north_west", "walking_west", "walking_south_west", "walking_south", "walking_south_east", "walking_east", "walking_north_east" };

    Animator animator;
    int lastDirection;
    string lastAnimationName = "";

    public GameObject pantsSlot;

    private void Awake()
    {
        //cache the animator component
        animator = GetComponent<Animator>();
    }


    public void SetDirection(Vector2 direction)
    {

        //use the Run states by default
        string[] directionArray = null;

        //measure the magnitude of the input.
        if (direction.magnitude < .01f)
        {
            //if we are basically standing still, we'll use the Static states
            //we won't be able to calculate a direction if the user isn't pressing one, anyway!
            directionArray = staticDirections;
        }
        else
        {
            //we can calculate which direction we are going in
            //use DirectionToIndex to get the index of the slice from the direction vector
            //save the answer to lastDirection
            directionArray = walkingDirections;
            lastDirection = DirectionToIndex(direction, 8);
        }

        Debug.Log(lastDirection);

        // if (directionArray.Length -1 == lastDirection)
        // {
        //     lastDirection = 0;
        // }
        // else
        // {
        //     lastDirection++;
        // }

        //tell the animator to play the requested state
        var animationName = directionArray[lastDirection];

        if (lastAnimationName == animationName)
        {
            return;
        }
        //play base character animation
        animator.Play(animationName, -1, 0);

        //play pants animation if exists
        if (pantsSlot != null)
        {
            var pantsAnimator = pantsSlot.GetComponentInChildren<Animator>();
            if (pantsAnimator != null)
            {
                pantsAnimator.Play(animationName, -1, 0);
            }
        }

        lastAnimationName = animationName;
    }

    //helper functions

    //this function converts a Vector2 direction to an index to a slice around a circle
    //this goes in a counter-clockwise direction.
    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        //get the normalized direction
        Vector2 normDir = dir.normalized;
        //calculate how many degrees one slice is
        float step = 360f / sliceCount;
        //calculate how many degress half a slice is.
        //we need this to offset the pie, so that the North (UP) slice is aligned in the center
        float halfstep = step / 2;
        //get the angle from -180 to 180 of the direction vector relative to the Up vector.
        //this will return the angle between dir and North.
        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        //add the halfslice offset
        angle += halfstep;
        //if angle is negative, then let's make it positive by adding 360 to wrap it around.
        if (angle < 0)
        {
            angle += 360;
        }
        //calculate the amount of steps required to reach this angle
        float stepCount = angle / step;
        //round it, and we have the answer!
        return Mathf.FloorToInt(stepCount);
    }







    //this function converts a string array to a int (animator hash) array.
    public static int[] AnimatorStringArrayToHashArray(string[] animationArray)
    {
        //allocate the same array length for our hash array
        int[] hashArray = new int[animationArray.Length];
        //loop through the string array
        for (int i = 0; i < animationArray.Length; i++)
        {
            //do the hash and save it to our hash array
            hashArray[i] = Animator.StringToHash(animationArray[i]);
        }
        //we're done!
        return hashArray;
    }
}
