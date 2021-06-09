using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private bool lerpPosition;
    private SpringyThingyController player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<SpringyThingyController>();
    }

    private void FixedUpdate()
    {
        if (lerpPosition)
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.AveragePosition, 0.1f) - Vector3.forward;
        else
            gameObject.transform.position = (Vector3)player.AveragePosition - Vector3.forward;
    }
}
