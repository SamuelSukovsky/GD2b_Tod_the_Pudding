using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed;
    public float minSpeed;
    public float damage;
    public AudioClip hitEnemy;
    public AudioClip hitAnythingElse;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.velocity = transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D context)
    {
        Enemy hit = context.gameObject.GetComponent<Enemy>();
        if(hit != null)
        {
            hit.Damage(damage);
            AudioManager.instance.PlaySound(hitEnemy);
        }
        else AudioManager.instance.PlaySound(hitAnythingElse);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (body.velocity.magnitude < minSpeed)
        {
            AudioManager.instance.PlaySound(hitAnythingElse);
            Destroy(gameObject);
        }
    }
}
