using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;                            // Publicly set variables
    public float dash = 40f;
    public float shootCooldown = 1f;
    public float shootCharge = .3f;
    public float dashCooldown = 5f;
    public float damage = 1f;
    public float health = 10f;
    public Camera cam;
    public GameObject sprite;
    public GameObject point;
    public GameObject projectile;

    public Vector2 dir;                                 // Internal variables, public for debug purposes
    public bool stationary = true;
    public bool isShooting = false;

    private float defaultZoom;                          // Private variables
    private float shootRecharge = 0f;
    private float shootChargeup;
    private float dashRecharge = 0f;
    private Vector2 mousePosition;
    private Rigidbody2D body;
    private Animator anim;
    private PlayerInput input;

    void Awake()                                        // On awake
    {
        body = GetComponent<Rigidbody2D>();                 // Get components
        anim = sprite.GetComponent<Animator>();
        defaultZoom = cam.orthographicSize;
        input = GetComponent<PlayerInput>();

        input.actions["AD"].started += AccelerateX;         // Assign functions to key binds
        input.actions["AD"].canceled += AccelerateX;
        input.actions["WS"].started += AccelerateY;
        input.actions["WS"].canceled += AccelerateY;
        input.actions["Space"].started += Dash;
        input.actions["MousePosition"].performed += GetMousePosition;
        input.actions["Mouseclick"].started += ToggleShooting;
        input.actions["Mouseclick"].canceled += ToggleShooting;

    }

    void FixedUpdate()                                  // On every frame
    {
        if (!stationary)                                    // If the player isn't stationary
        {
            body.AddForce(dir * speed);                         // Add force
            cam.orthographicSize = defaultZoom + Mathf.Sqrt(body.velocity.magnitude);
                                                                // If the player is moving very slow and they didn't just dash
            if (body.velocity.magnitude < 2f && dashRecharge != dashCooldown)  
            {
                stationary = true;                                  // Make the player stationary
                anim.SetBool("Moving", false);
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

        if (dashRecharge > 0f)                              // If dash is on cooldown
        {
            dashRecharge -= Time.deltaTime;                     // Countdown dash cooldown
        }
    }

    void Update()
    {
        if (!isShooting)
        {
            sprite.transform.right = point.transform.localPosition;
        }
        else
        {
            sprite.transform.right = (Vector2) cam.ScreenToWorldPoint(mousePosition) - (Vector2) transform.position;
            if (shootRecharge <= 0f)
            {
                if (shootChargeup == shootCharge)
                {
                    anim.SetBool("Charge", true);
                }

                if (shootChargeup <= 0f)
                {
                    Shoot();
                }
                else
                {
                    shootChargeup -= Time.deltaTime;
                }
            }
        }

        if (shootRecharge > 0f)                              // If dash is on cooldown
        {
            shootRecharge -= Time.deltaTime;                     // Countdown dash cooldown
        }
    }

    private void GetMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
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

    void ToggleShooting(InputAction.CallbackContext context)
    {
        isShooting = !isShooting;
        shootChargeup = shootCharge;
        anim.SetBool("Charge", false);
    }

    void Shoot()
    {
        anim.ResetTrigger("Shoot");
        anim.SetTrigger("Shoot");
        anim.SetBool("Charge", false);
        GameObject shot = Instantiate(projectile, sprite.transform.position, sprite.transform.rotation);
        shot.GetComponent<Rigidbody2D>().velocity += body.velocity;
        shot.GetComponent<Projectile>().damage = damage;
        shootRecharge = shootCooldown;
        shootChargeup = shootCharge;
    }

    public void Damage(float damageTaken)
    {
        health -= damageTaken;
    }

    void OnDestroy()                                    // On destroy (for good practice)
    {
        input.actions["AD"].started -= AccelerateX;         // Unassign functions from key binds
        input.actions["AD"].canceled -= AccelerateX;
        input.actions["WS"].started -= AccelerateY;
        input.actions["WS"].canceled -= AccelerateY;
        input.actions["Space"].started -= Dash;
        input.actions["MousePosition"].performed -= GetMousePosition;
        input.actions["Mouseclick"].started -= ToggleShooting;
        input.actions["Mouseclick"].canceled -= ToggleShooting;
    }
}