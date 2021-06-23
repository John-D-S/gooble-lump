using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainRotation : MonoBehaviour
{
    void Update()
    {
        //set the rotation of the gameobject to always be 0
        gameObject.transform.rotation = Quaternion.identity;
    }
}
