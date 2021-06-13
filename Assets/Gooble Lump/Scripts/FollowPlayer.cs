using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private bool lerpPosition;
    [SerializeField]
    private bool rotateWithPlayer;
    private SpringyThingyController player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<SpringyThingyController>();
        gameObject.transform.position = (Vector3)player.AveragePosition - Vector3.forward;
    }

    private void FixedUpdate()
    {
        if (lerpPosition)
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.AveragePosition, 0.1f) - Vector3.forward;
        else
            gameObject.transform.position = (Vector3)player.AveragePosition - Vector3.forward;

        if (rotateWithPlayer)
            gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * player.AverageZRotation);
    }
}
