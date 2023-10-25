using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D body;                               // Variables
    public float speed;
    public float minSpeed;
    public float damage;
    public AudioClip hitEnemy;
    public AudioClip hitAnythingElse;

    void Awake()                                            // Before first frame
    {
        body = GetComponent<Rigidbody2D>();                     // Get rigidbody
        body.velocity = transform.right * speed;                // Accelerate forward
    }

    void OnCollisionEnter2D(Collision2D context)            // On collision
    {
        Enemy hit = context.gameObject.GetComponent<Enemy>();   // Check if the object hit is an enemy
        if(hit != null)                                         // If yes
        {
            hit.Damage(damage);                                     // Damage target
            AudioManager.instance.PlaySound(hitEnemy);              // Play hit sound
        }
        else AudioManager.instance.PlaySound(hitAnythingElse);  // Else play miss sound
        Destroy(gameObject);                                    // Destroy self
    }

    void FixedUpdate()                                      // Each frame
    {
        if (body.velocity.magnitude < minSpeed)                 // If slower than min speed
        {
            AudioManager.instance.PlaySound(hitAnythingElse);       // Play miss sound
            Destroy(gameObject);                                    // Destroy self
        }
    }
}
