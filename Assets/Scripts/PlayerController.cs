using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;                                // Publicly set variables
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
    public GameObject panel;

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

        input.actions["Pause"].started += TogglePause;
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

    void Update()                                       // Every frame:
    {
        if (!GameManager.instance.paused)                       // If the game isn't paused
        {
            if (!isShooting)                                    // If the player isn't shooting
            {                                                       // Turn in the direction of movement
                sprite.transform.right = point.transform.localPosition;
            }
            else                                                // Otherwise
            {                                                       // Look at the mouse
                sprite.transform.right = (Vector2) cam.ScreenToWorldPoint(mousePosition) - (Vector2) transform.position;
                if (shootRecharge <= 0f)                            // If shooting is recharged
                {
                    if (shootChargeup == shootCharge)                   // If begining charge
                    {
                        anim.SetBool("Charge", true);                       // Start charge animation
                    }
                    
                    if (shootChargeup <= 0f)                            // If finished charging
                    {
                        Shoot();                                            // Shoot
                    }
                    else                                                // Else
                    {
                        shootChargeup -= Time.deltaTime;                    // Continue charging
                    }
                }
            }

            if (shootRecharge > 0f)                              // If dash is on cooldown
            {
                shootRecharge -= Time.deltaTime;                     // Countdown dash cooldown
            }
        }
    }
    
    private void GetMousePosition(InputAction.CallbackContext context)
    {                                                   // Get mouse position
        mousePosition = context.ReadValue<Vector2>();       // Read mouse position
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
                                                        // Toggle shooting 
    void ToggleShooting(InputAction.CallbackContext context)
    {
        isShooting = !isShooting;                           // Change shooting status
        shootChargeup = shootCharge;                        // Reset shooting charge
        anim.SetBool("Charge", false);                      // Stop the charge animation
    }

    void Shoot()                                        // Shoot
    {
        anim.ResetTrigger("Shoot");                         // Reset animation trigger
        anim.SetTrigger("Shoot");                           // Trigger shoot animation
        anim.SetBool("Charge", false);                      // stop charge animation
                                                            // Instatiate new projectile and add player velocity
        GameObject shot = Instantiate(projectile, sprite.transform.position, sprite.transform.rotation);
        shot.GetComponent<Rigidbody2D>().velocity += body.velocity;
        shot.GetComponent<Projectile>().damage = damage;    // Give the projectile damage
        shootRecharge = shootCooldown;                      // Reset shooting cooldown
        shootChargeup = shootCharge;                        // Reset shooting charge
    }

    public void Damage(float damageTaken)               // Take damage
    {
        health -= damageTaken;                              // Lower health by damage value
        anim.ResetTrigger("Hurt");                          // Reset animation trigger
        anim.SetTrigger("Hurt");                            // Trigger hurt animation
        if (health <= 0f)                                   // If health is zero or less
        {
            anim.SetBool("Dead", true);                         // Start death animation
            UnassignControls();                                 // Remove control
            GameManager.instance.GameOver();                    // End game
        }
    }

    void TogglePause(InputAction.CallbackContext context)
    {                                                   // Pause/unpause
        if(GameManager.instance.paused)                     // If game paused, unpause it
        {
            GameManager.instance.ResumeGame(panel);
        }
        else                                                // Else, pause it
        {
            GameManager.instance.PauseGame(panel);
        }
    }

    void UnassignControls()                             // Unassign functions from key binds
    {
        input.actions["AD"].started -= AccelerateX;
        input.actions["AD"].canceled -= AccelerateX;
        input.actions["WS"].started -= AccelerateY;
        input.actions["WS"].canceled -= AccelerateY;
        input.actions["Space"].started -= Dash;
        input.actions["MousePosition"].performed -= GetMousePosition;
        input.actions["Mouseclick"].started -= ToggleShooting;
        input.actions["Mouseclick"].canceled -= ToggleShooting;

        input.actions["Pause"].started -= TogglePause;
    }
    void OnDestroy()                                    // On destroy (for good practice)
    {
        UnassignControls();
    }
}