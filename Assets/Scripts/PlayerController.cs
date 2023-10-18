using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float dash = 40f;
    public float dashCooldown = 5f;
    private float dashRecharge = 0f;
    public Vector2 dir;
    public bool stationary = true;
    private Rigidbody2D body;
    private Animator anim;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions["AD"].started += AccelerateX;
        input.actions["AD"].canceled += AccelerateX;
        input.actions["WS"].started += AccelerateY;
        input.actions["WS"].canceled += AccelerateY;
        input.actions["Space"].started += Dash;
    }

    void FixedUpdate()
    {
        if (!stationary)
        {
            body.AddForce(dir * speed);
            
            if (body.velocityX * body.velocityX + body.velocityY * body.velocityY < 1)
            {
                stationary = true;
                body.velocityX = 0f;
                body.velocityY = 0f;
                Debug.Log(body.velocityX * body.velocityX + body.velocityY * body.velocityY + "F");
            }
        }
        if (dashRecharge > 0f)
        {
            dashRecharge -= Time.deltaTime;
        }
    }

    void AccelerateX(InputAction.CallbackContext context)
    {
        dir.x = context.ReadValue<float>();
        if (dir.x != 0f && dir.y != 0f)
        {
            dir.Set(Mathf.Sqrt(.5f) * dir.x, Mathf.Sqrt(.5f) * dir.y);
        }
        else if (Mathf.Abs(dir.y) < 1f && dir.y != 0f)
        {
            if (dir.y > 0f)
            {
                dir.y = 1f;
            }
            else dir.y = -1f;
        }
    }

    void AccelerateY(InputAction.CallbackContext context)
    {
        dir.y = context.ReadValue<float>();
        if (dir.y != 0f && dir.x != 0f)
        {
            dir.Set(Mathf.Sqrt(.5f) * dir.x, Mathf.Sqrt(.5f) * dir.y);
        }
        else if (Mathf.Abs(dir.x) < 1f && dir.x != 0f)
        {
            if (dir.x > 0f)
            {
                dir.x = 1f;
            }
            else dir.x = -1f;
        }
    }

    void Dash(InputAction.CallbackContext context)
    {
        if (dashRecharge <= 0f)
        {
            stationary = false;
            body.velocity = body.velocity + dir * dash;
            dashRecharge = dashCooldown;
        }
    }
}
