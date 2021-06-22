using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Disappear : MonoBehaviour
{
    [SerializeField, Tooltip("The time after contact that the platform dissapears")]
    private float maxTime = 0.25f;
    //the float that counts down when the player leaves the platform
    private float timer;
    //this determines whether the timer is counting down
    private bool timerActivated = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if the player hits the platform and is effected, the timer will be set to max time and will be activated
        if (collision.enabled && collision.collider.tag == "Player")
        {
            timer = maxTime;
            timerActivated = true;
        }
    }

    private void FixedUpdate()
    {
        //if the timer is activated, count down to 0
        if (timerActivated)
        {
            timer -= Time.fixedDeltaTime;
            //if the timer reaches 0, destroy the platform
            if (timer < 0)
                Destroy(gameObject);
        }
    }
}
