using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable), typeof(Interactable))]
public class Tree : MonoBehaviour, IDestructable
{
    [SerializeField]
    private float healthAmount;
    private bool isDead;
    private Quaternion initialRotation;

    public int lumberCount = 10;
    public float HealthAmount
    {
        get
        {
            return healthAmount;
        }

        set
        {
            healthAmount = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }


    void Start()
    {
        initialRotation = transform.rotation;
    }

    public void TakeDamage(float damageAmount)
    {
        var interactableComponent = GetComponent<Interactable>();
        if (interactableComponent.IsCloseEnough())
        {
            healthAmount -= damageAmount;
            if (healthAmount <= 0)
            {
                IsDead = true;
                StartCoroutine(TurnOverAnimation());
                //transform.eulerAngles = new Vector3(0, 0, 45);
                //Destroy(gameObject);
            }
        }
    }



    IEnumerator TurnOverAnimation()
    {
        var t = 0.0f;

        // Just make the animation interval configurable for easier modification later
        const float animationInterval = 0.01f;

        // Loop until instructed otherwise
        while (true)
        {
            t += Time.deltaTime / 3;
            // Do some nice animation
            //transform.Rotate(Vector3.forward * (200 * Time.deltaTime));
            var time = Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t));
            transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(0, 0, 90), time);

            // Make the coroutine wait for a moment
            yield return new WaitForSeconds(animationInterval);

            if (transform.rotation.eulerAngles.z >= 90)
            {
                // Break the while loop, NOT the coroutine
                break;
            }
        }

        // Finally destroy tree      
        DestroyTree();
    }

    void DestroyTree()
    {

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        var height = sr.sprite.textureRect.height / 100;//100 because we consider each unit is 100px
        var initialPosition = transform.position;
        Destroy(gameObject);

        var dropComponent = GetComponent<DropOnDeath>();
        for (int i = 0; i < dropComponent.dropCount; i++)
        {
            var eachOffSet = height / dropComponent.dropCount;
            dropComponent.DropItem(new Vector3(initialPosition.x - (eachOffSet * i/4), initialPosition.y, initialPosition.z));
            //dropComponent.DropItem(initialPosition);
        }
    }
}
