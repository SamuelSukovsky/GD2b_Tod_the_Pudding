using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D body;
    public float damage;

    public void Set(Vector2 velocity, float dmg)
    {
        body.velocity = velocity;
        damage = dmg;
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
