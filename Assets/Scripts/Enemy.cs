using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    public float health;                                // Variables
    public float speed;
    public float damage;
    public int points;
    public GameObject target;
    public Rigidbody2D body;
    public Animator anim;

    void Awake()                                        // Before first frame
    {
        body = GetComponent<Rigidbody2D>();                 // Get components
        anim = GetComponent<Animator>();
    }

    public void Damage(float damageTaken)               // Take damage
    {
        health -= damageTaken;                              // Lower health by damage taken
        if (health <= 0f)                                   // If health is zero or less
        {
            Die();                                              // Die
        }
    }

    protected void MoveTo(Vector2 targetPos)            // Currently unused
    {
        body.velocity = (targetPos - (Vector2) transform.position).normalized * speed;
    }

    protected virtual void Die()                        // Default Die function that is overriden by subclasses
    {
        Destroy(gameObject);
    }
}
