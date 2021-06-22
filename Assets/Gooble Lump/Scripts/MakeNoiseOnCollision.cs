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

    private float pastVelocity = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.enabled && !audioSource.isPlaying)
        {
            float audioSourceVolume = -Mathf.Pow(2, -pastVelocity * collisionVelocityMultiplier) + 1;
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

    private IEnumerator UpdatePastVelocity(int fixedUpdatesToWait)
    {
        float storedVelocity = rigidbody.velocity.magnitude;
        for (int i = 0; i < fixedUpdatesToWait; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        pastVelocity = storedVelocity;
    }

    private void FixedUpdate()
    {
        StartCoroutine(UpdatePastVelocity(3));
    }
}
