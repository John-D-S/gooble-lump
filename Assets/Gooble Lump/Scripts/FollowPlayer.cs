using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("-- Following Player Options --")]
    [SerializeField, Tooltip("whether or not to lerp towards the player's position")]
    private bool lerpPosition;
    [SerializeField, Tooltip("whether or not to allign rotation with player's rotation")]
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
        //lerp or set postion to player's position
        if (lerpPosition)
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.AveragePosition, 0.1f) - Vector3.forward;
        else
            gameObject.transform.position = (Vector3)player.AveragePosition - Vector3.forward;

        //if rotatewithPlayer, rotate with player
        if (rotateWithPlayer)
            gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * player.AverageZRotation);
    }
}
