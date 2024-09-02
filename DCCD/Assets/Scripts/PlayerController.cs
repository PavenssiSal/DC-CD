using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Globaali muuttuja
    public static PlayerController instance;

    [Header("KARTTA")]
    public GameObject initialMap;

    [Header("LIIKKUMINEN")]
    public bool canMove;
    public float speed = 2f;
    private Rigidbody2D rb;
    private Vector2 mov;

    public float dashDistance = 5f;    // Distance covered during the dash
    public float dashDuration = 0.2f;  // Duration of the dash in seconds
    public float dashCooldown = 2f;    // Cooldown time between dashes

    private bool canDash = true;       // Indicates if the player can dash
    private bool isDashing = false;    // Tracks if the player is currently dashing

    private void Start()
    {
        //aseta aloituskartan rajat
        Camera.main.GetComponent<MainCamera>().SetBound(initialMap);
        //Luodaan yhteys physiikkamoottoriin, jotta pelihahmoa voidaan liikuttaa
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //Pelihahmo voi liikkua
        canMove = true;
    }

    private Animator anim;

    private void Update()
    {
        Vector2 dir = Vector2.zero;

        // Movement Input
        if (!isDashing) // Disable normal movement during dash
        {
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                
            }

            dir.Normalize();
            //animator.SetBool("IsMoving", dir.magnitude > 0);

            // Normal movement
            rb.velocity = speed * dir;
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(Dash(dir));
        }
    }

    private void Animations()
    {
        if (canMove && mov.magnitude !=0)
        {
            anim.SetFloat("MoveX", mov.x);
            anim.SetFloat("MoveY", mov.y);
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }
    private IEnumerator Dash(Vector2 direction)
    {
        if (direction.magnitude == 0)
            yield break;  // Prevent dashing with no direction

        // Disable movement and further dashing until the dash is complete
        canDash = false;
        isDashing = true;

        // Store the initial velocity so it can be restored after the dash
        Vector2 initialVelocity = rb.velocity;

        // Calculate the speed required to cover the dash distance within the dash duration
        float dashSpeed = dashDistance / dashDuration;
        rb.velocity = direction * dashSpeed;

        // // Dash animation placeholder
        // animator.SetTrigger("Dash");

        // Wait for the duration of the dash
        yield return new WaitForSeconds(dashDuration);

        // Restore normal movement
        rb.velocity = initialVelocity;

        // End the dash
        isDashing = false;

        // Wait for the cooldown duration before enabling dash again
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
