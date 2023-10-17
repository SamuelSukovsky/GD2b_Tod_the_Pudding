using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 400f;
    public Vector2 dir;
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
    }

    void Update()
    {
        body.AddForce(dir * speed);
    }

    void AccelerateX(InputAction.CallbackContext context)
    {
        dir.x = context.ReadValue<float>();
        Debug.Log("MOVE!!!!");
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
        Debug.Log("MOVE!!!!");
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
}
