using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evil : Enemy
{
    void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        body.velocity = transform.right * speed;
    }

    protected override void Die()
    {

    }
}
