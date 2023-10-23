using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public int points;
    public GameObject target;
    protected Rigidbody2D body;
    protected Animator anim;

    protected void Activate()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            anim.SetBool("Alive", false);
            Die();
        }
    }

    protected void MoveTo(Vector2 targetPos)
    {
        body.velocity = (targetPos - (Vector2) transform.position).normalized * speed;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
