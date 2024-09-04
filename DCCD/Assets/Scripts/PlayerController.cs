using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("KARTTA")]
    public GameObject initialMap;

    [Header("LIIKKUMINEN")]
    public bool canMove;
    public float speed = 2f;
    
    public float dashDistance = 5f;    // Distance covered during the dash
    public float dashDuration = 0.2f;  // Duration of the dash in seconds
    public float dashCooldown = 2f;    // Cooldown time between dashes

    private bool canDash = true;       // Indicates if the player can dash
    private bool isDashing = false;    // Tracks if the player is currently dashing

    [Header("ATTACK")]
    public float attackDuration = 0.5f;  // Duration of the attack animation
    public float attackRange = 3.0f;

    [Header("ANIMATOR")]
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 movement;
    private void Start()
    {
        Camera.main.GetComponent<MainCamera>().SetBound(initialMap);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;
    }

    private void Update()
    {
        Vector2 dir = Vector2.zero;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 ||
       Input.GetAxisRaw("Horizontal") == -1 ||
       Input.GetAxisRaw("Vertical") == 1 ||
       Input.GetAxisRaw("Vertical") == -1)
        {
            anim.SetFloat("Last_Horizontal",
                              Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("Last_Vertical",
                              Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetKeyDown(KeyCode.Space))
           anim.SetBool("isAttack", true);

        if (!isDashing) // Disable normal movement during dash
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                dir.x = -1;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                dir.x = 1;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                dir.y = 1;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                dir.y = -1;
            }

            dir.Normalize();
            rb.velocity = speed * dir;
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(Dash(dir));
        }

        // Attack input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1))  // Assuming space is the attack key
        {
            StartCoroutine(PerformAttack());
        }
    }

    void AttackAnimation()
    {
        if (anim.GetBool("isAttack") == true)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.MovePosition(rb.position + movement * speed *
                            Time.fixedDeltaTime);
        }
    }

    void StopAttack()
    {
        if (anim.GetBool("isAttack"))
            anim.SetBool("isAttack", false);
    }
    private IEnumerator PerformAttack()
    {
        // Check if attack is allowed
        if (!canMove || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield break;

        // Prevent movement during attack
        canMove = false;

        // Set the attack animation trigger based on the direction the player last faced
        anim.SetBool("isAttack", true);

        // Perform the attack based on the last facing direction
        Vector2 attackPosition = rb.position;
        Vector2 attackDirection = new Vector2(anim.GetFloat("Last_Horizontal"), anim.GetFloat("Last_Vertical")).normalized;

        // Define the area of attack (could use OverlapBox, Circle, or Raycast depending on the setup)
        RaycastHit2D[] hitEnemies = Physics2D.RaycastAll(attackPosition, attackDirection, attackRange);

        // Process the hits, applying damage if necessary
        foreach (RaycastHit2D hit in hitEnemies)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                // Damage the enemy (assuming enemy has a TakeDamage method)
                //hit.collider.GetComponent<Enemy>().TakeDamage(1);  // Assuming 1 damage
                Debug.Log("You hit something");
            }
        }

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(attackDuration);

        // Reset attack state
        anim.SetBool("isAttack", false);
        canMove = true;
    }


    //Probably not atleast fully me
    private IEnumerator Dash(Vector2 direction)
    {
        if (direction.magnitude == 0)
            yield break;  // Prevent dashing with no direction

        canDash = false;
        isDashing = true;
        Vector2 initialVelocity = rb.velocity;
        float dashSpeed = dashDistance / dashDuration;
        rb.velocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = initialVelocity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
