using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evil : Enemy                           // Evil extends Enemy
{
    void FixedUpdate()                                  // Every frame
    {
        if (health > 0f)                                    // If alive
        {                                                       // Turn towards the player
            transform.right = target.transform.position - transform.position;
            body.velocity = transform.right * speed;            // Move forward
        }
    }

    void OnCollisionEnter2D(Collision2D context)        // On collision
    {                                                       // Attempt to find playerController script on target
        PlayerController hit = context.gameObject.GetComponent<PlayerController>();
        if(hit != null)                                     // If succesful
        {
            hit.Damage(damage);                                 // Damage the player
            health = 0f;                                        // Die
            Die();
        }
    }

    override protected void Die()                       // Death
    {
        anim.SetBool("Alive", false);                       // Start death animation
        Destroy(body);                                      // Remove rigidbody
        Destroy(GetComponent<Collider2D>());                // Remove collider
        Destroy(gameObject, 3f);                            // Destroy self after three seconds
    }
}
