using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField]
    float MaxRotationOffset = 90;
    
    void Start()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, Random.Range(-MaxRotationOffset, MaxRotationOffset)));
    }
}