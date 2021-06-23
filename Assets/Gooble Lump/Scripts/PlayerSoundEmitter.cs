using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static StaticObjectHolder;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundEmitter : MonoBehaviour
{
    [Header("-- SoundSettings --")]
    [SerializeField, Tooltip("The sound the player makes when extending")]
    private AudioClip extendSound;
    [SerializeField, Tooltip("The sound the player makes when retracting")]
    private AudioClip retractSound;

    private AudioSource audioSource;

    bool wasPlayerExtendedLastFrame;

    private void Start()
    {
        //set the audio source component
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        //if the player extends or retracts, play the appropriate sound.
        if (player.isExtended != wasPlayerExtendedLastFrame)
        {
            audioSource.clip = player.isExtended ? extendSound : retractSound;
            audioSource.Play();
            wasPlayerExtendedLastFrame = player.isExtended;
        }
    }
}
