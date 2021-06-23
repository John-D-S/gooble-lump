using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [Header("-- Random Rotation Settings --")]
    [SerializeField, Tooltip("The Maximum Rotation offset That the gameObject can have upon being instantiated")]
    float MaxRotationOffset = 90;
    
    void Start()
    {
        //apply the random rotation
        gameObject.transform.Rotate(new Vector3(0, 0, Random.Range(-MaxRotationOffset, MaxRotationOffset)));
    }
}