using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource), typeof(Collider2D))]
public class MakeNoiseOnCollision : MonoBehaviour
{
    [SerializeField]
    private float collisionVelocityMultiplier = 1;

    private AudioSource audioSource;
    private Collider2D collider;
    private Rigidbody2D rigidbody;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.enabled && !audioSource.isPlaying)
        {
            float velocityAtPoint = rigidbody.GetPointVelocity(collision.GetContact(0).point).magnitude * collisionVelocityMultiplier;
            float audioSourceVolume = -Mathf.Pow(2, -velocityAtPoint) + 1;
            audioSource.volume = audioSourceVolume;
            audioSource.Play();
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        collider = GetComponentInParent<Collider2D>();
        rigidbody = collider.attachedRigidbody;
    }
}
