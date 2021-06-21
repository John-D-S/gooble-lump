using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static StaticObjectHolder;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundEmitter : MonoBehaviour
{
    [SerializeField]
    private AudioClip extendSound;
    [SerializeField]
    private AudioClip retractSound;

    private AudioSource audioSource;

    bool wasPlayerExtendedLastFrame;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (player.isExtended != wasPlayerExtendedLastFrame)
        {
            audioSource.clip = player.isExtended ? extendSound : retractSound;
            audioSource.Play();
            wasPlayerExtendedLastFrame = player.isExtended;
        }
    }
}
