using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticObjectHolder;

[RequireComponent(typeof(ParticleSystem))]
public class PlayerParticles : MonoBehaviour
{
    ParticleSystem playerParticleSystem;
    private ParticleSystem.ShapeModule shape;
    private ParticleSystem.ExternalForcesModule externalForces;

    private void Start()
    {
        playerParticleSystem = GetComponent<ParticleSystem>();
        shape = playerParticleSystem.shape;
        externalForces = playerParticleSystem.externalForces;
    }

    private void FixedUpdate()
    {
        shape.position = player.AveragePosition;
    }
}
