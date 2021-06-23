using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource), typeof(Collider2D))]
public class MakeNoiseOnCollision : MonoBehaviour
{
    [SerializeField, Tooltip("How much the velocity of the rigidbody effects the sound volume")]
    private float collisionVelocityMultiplier = 1;

    private AudioSource audioSource;
    private Collider2D thisCollider;
    private Rigidbody2D thisRigidbody;

    private float pastVelocity = 0;

    /// <summary>
    /// This coroutine sets the past velocity to the velocity of the rigidbody fixedUpdatesToWait
    /// </summary>
    /// <param name="fixedUpdatesToWait">The number of FixedUpdates that the pastVelocity is taken From</param>
    private IEnumerator UpdatePastVelocity(int fixedUpdatesToWait)
    {
        float storedVelocity = thisRigidbody.velocity.magnitude;
        for (int i = 0; i < fixedUpdatesToWait; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        pastVelocity = storedVelocity;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the player collides with a platform and an the audio source is not playing
        if (collision.enabled && !audioSource.isPlaying)
        {
            //the volume of the audio source approaches 1 from 0 as the velocity of the rigidbody increases
            //put -2^(-x)+1 into a graphing calculator to visualize it
            //pastVelocity is used because the current velocity is already affected by the collision.
            float audioSourceVolume = -Mathf.Pow(2, -pastVelocity * collisionVelocityMultiplier) + 1;
            //set the volume
            audioSource.volume = audioSourceVolume;
            //play the sound
            audioSource.Play();
        }
    }

    private void Start()
    {
        //setting the components
        audioSource = GetComponent<AudioSource>();
        thisCollider = GetComponentInParent<Collider2D>();
        thisRigidbody = thisCollider.attachedRigidbody;
    }

    private void FixedUpdate()
    {
        //update the past velocity every fixed update.
        StartCoroutine(UpdatePastVelocity(3));
    }
}
