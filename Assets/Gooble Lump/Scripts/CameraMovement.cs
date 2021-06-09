using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private SpringyThingyController player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<SpringyThingyController>();
    }

    private void FixedUpdate()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.AveragePosition, 0.1f) - Vector3.forward;
    }
}
