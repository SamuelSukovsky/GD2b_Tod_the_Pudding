using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;                            // Publicly set variables
    public float dash = 40f;
    public float dashCooldown = 5f;
    private float dashRecharge = 0f;
    public Camera cam;
    public GameObject sprite;
    public GameObject point;
    public GameObject projectile;

    public float velocity;                              // Internal variables, public for debug purposes
    public Vector2 dir;
    public bool stationary = true;

    private float defaultZoom;                          // Private variables
    private Rigidbody2D body;
    private Animator anim;

    void Awake()                                        // On awake
    {
        body = GetComponent<Rigidbody2D>();                 // Get components
        anim = sprite.GetComponent<Animator>();
        defaultZoom = cam.orthographicSize;
        PlayerInput input = GetComponent<PlayerInput>();

        input.actions["AD"].started += AccelerateX;         // Assign functions to key binds
        input.actions["AD"].canceled += AccelerateX;
        input.actions["WS"].started += AccelerateY;
        input.actions["WS"].canceled += AccelerateY;
        input.actions["Space"].started += Dash;
    }

    void FixedUpdate()                                  // On every frame
    {
        if (!stationary)                                    // If the player isn't stationary
        {
            body.AddForce(dir * speed);                         // Add force
                                                                // Calculate velocity and move camera
            velocity = Mathf.Sqrt(body.velocityX * body.velocityX + body.velocityY * body.velocityY);
            cam.orthographicSize = defaultZoom + Mathf.Sqrt(velocity);

            if (velocity < 2f && dashCooldown != dashRecharge)  // If the player is moving very slow and they didn't just dash
            {
                stationary = true;                                  // Make the player stationary
                anim.SetBool("Moving", false);
                velocity = 0;
                body.velocityX = 0f;
                body.velocityY = 0f;
                dashRecharge = 0f;
            }
            else                                                // Else
            {                                                       // Place aim point in the direction of movement
                point.transform.localPosition = new Vector3(body.velocityX, body.velocityY, 0f);
            }
        }
        else if (!dir.Equals(new Vector2(0f, 0f)))          // Else if direction has input
        {                                                       // Place aim point in the direction
            point.transform.localPosition = new Vector3(dir.x, dir.y, 0f);
        }
        sprite.transform.right = point.transform.localPosition;

        if (dashRecharge > 0f)                              // If dash is on cooldown
        {
            dashRecharge -= Time.deltaTime;                     // Countdown dash cooldown
        }
    }

    // Accelerate functions
    void AccelerateX(InputAction.CallbackContext context)
    {
        dir.x = context.ReadValue<float>();                 // Set the x direction to x axis input value
        if (dir.x != 0f && dir.y != 0f)                     // If direction is diagonal
        {                                                       // Calculate the direction values
            dir.Set(Mathf.Sqrt(.5f) * dir.x, Mathf.Sqrt(.5f) * dir.y);
        }
        else if (dir.y != 0f)                               // Else if y direction is not zero
        {
            if (dir.y > 0f)                                     // Reset y direction to full
            {
                dir.y = 1f;
            }
            else dir.y = -1f;
        }
    }

    void AccelerateY(InputAction.CallbackContext context)
    {
        dir.y = context.ReadValue<float>();                 // Set the y direction to y axis input value
        if (dir.y != 0f && dir.x != 0f)                     // If direction is diagonal
        {                                                       // Calculate the direction values
            dir.Set(Mathf.Sqrt(.5f) * dir.x, Mathf.Sqrt(.5f) * dir.y);
        }
        else if (dir.x != 0f)                               // Else if x direction is not zero
        {
            if (dir.x > 0f)                                     // Reset x direction to full
            {
                dir.x = 1f;
            }
            else dir.x = -1f;
        }
    }

    void Dash(InputAction.CallbackContext context)      // Dash Function
    {
        if (dashRecharge <= 0f)                             // If dash isn't on cooldown
        {
            stationary = false;                                 // The player isn't stationary
            anim.SetBool("Moving", true);                       // Trigger moving animation
            body.AddForce(dir * dash);                          // Add force in target direction
            dashRecharge = dashCooldown;                        // Put dash on cooldown
        }
    }

    void Shoot(InputAction.CallbackContext context)
    {
        GameObject shot = Instantiate(projectile);
    }
}