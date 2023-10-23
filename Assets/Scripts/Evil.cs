using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evil : Enemy
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if (health > 0f)
        {
            body.velocity = transform.right * speed;
            transform.right = target.transform.position - transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D context)
    {
        PlayerController hit = context.gameObject.GetComponent<PlayerController>();
        if(hit != null)
        {
            hit.Damage(damage);
            health = 0f;
            Die();
        }
    }

    override protected void Die()
    {
        anim.SetBool("Alive", false);
        Destroy(body);
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, 3f);
    }
}
