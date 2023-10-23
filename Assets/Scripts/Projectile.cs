using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed;
    public float minSpeed;
    public float damage;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (body.velocity.magnitude < minSpeed)
        {
            Destroy(gameObject);
        }
    }
}
