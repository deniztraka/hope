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
    private SmoothSineMovement sineMovement;
    private int directionMultiplier = 1;
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
    public AudioSource audioSource;
    public AudioClip chopAudioClip;
    public AudioClip fallAudioClip;
    public float volLowRange = .5f;
    public float volHighRange = 1f;


    void Start()
    {
        initialRotation = transform.rotation;
        audioSource = GetComponent<AudioSource>();
        sineMovement = GetComponent<SmoothSineMovement>();
        sineMovement.enabled = false;
    }
    public void TakeDamage(float damageAmount)
    {
        var results = Resources.FindObjectsOfTypeAll<ExitGameCanvasPopUp>();
        var exitGameCanvas = results[0].gameObject;
        exitGameCanvas.SetActive(true);


        if (isDead || healthAmount <= 0)
        {
            return;
        }

        var interactableComponent = GetComponent<Interactable>();
        if (!interactableComponent.IsCloseEnough())
        {
            return;
        }

        healthAmount -= damageAmount;

        StartCoroutine(WaitAndChoppedAnimation(0.5f));

        if (healthAmount <= 0)
        {
            IsDead = true;

            Fall();
        }
    }

    private void PlayChopSoundEffect()
    {
        float vol = Random.Range(volLowRange, volHighRange);
        audioSource.PlayOneShot(chopAudioClip, vol);
    }

    private void Fall()
    {
        StartCoroutine(WaitAndFall(1f));
    }

    private void PlayFallSoundEffect()
    {
        float vol = Random.Range(volLowRange, volHighRange);
        audioSource.PlayOneShot(fallAudioClip, vol);
    }

    IEnumerator WaitAndChoppedAnimation(float seconds)
    {
        PlayChopSoundEffect();
        yield return new WaitForSeconds(seconds);
        StartCoroutine(ChoppedAnimation());

    }

    IEnumerator ChoppedAnimation()
    {
        sineMovement.enabled = true;
        yield return new WaitForSeconds(1.0f);
        sineMovement.enabled = false;
    }

    IEnumerator WaitAndFall(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //to show it in front of trees
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -1.5f);
        StartCoroutine(FallingAnimation());
        PlayFallSoundEffect();
    }

    IEnumerator FallingAnimation()
    {

        var player = GameObject.FindWithTag("Player");
        if (player.transform.position.x - transform.position.x < 0)
        {
            directionMultiplier = -1;
        };


        var t = 0.2f;
        // Just make the animation interval configurable for easier modification later
        const float animationInterval = 0.025f;

        // Loop until instructed otherwise
        while (true)
        {
            t += Time.deltaTime / 3;
            var time = Mathf.SmoothStep(1.0f, 0.0f, Mathf.SmoothStep(1.0f, 0.0f, t));
            transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(0, 0, directionMultiplier * 90), time);

            // Make the coroutine wait for a moment
            yield return new WaitForSeconds(animationInterval);

            if ((directionMultiplier > 0 && transform.rotation.eulerAngles.z >= 90) || (directionMultiplier < 0 && transform.rotation.eulerAngles.z <= 270))
            {
                // Break the while loop, NOT the coroutine
                break;
            }
        }

        OnFallingAnimationFinished();
    }

    private void OnFallingAnimationFinished()
    {
        DropItems();
        Destroy(gameObject);
    }

    private void DropItems()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        var height = sr.sprite.textureRect.height / 100;//100 because we consider each unit is 100px
        var initialPosition = transform.position;
        var dropComponent = GetComponent<DropOnDeath>();
        for (int i = 0; i < dropComponent.dropCount; i++)
        {
            var eachOffSet = height / dropComponent.dropCount;
            dropComponent.DropItem(new Vector3(initialPosition.x - (directionMultiplier * (eachOffSet * i / 5)), initialPosition.y, initialPosition.z));
        }
    }
}
