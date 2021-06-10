using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Disappear : MonoBehaviour
{
    private float maxTime = 0.25f;
    private float timer;
    private bool timerActivated = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            timer = maxTime;
            timerActivated = true;
        }
    }

    private void FixedUpdate()
    {
        if (timerActivated)
        {
            timer -= Time.fixedDeltaTime;
            if (timer < 0)
                Destroy(gameObject);
        }
    }
}
