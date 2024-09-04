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
    public GameObject swordSwingPrefab;  // Prefab for the sword swing
    public float attackRange = 1f;       // Range of the attack
    public float attackDuration = 0.5f;  // Duration of the attack animation

    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 attackDirection;

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
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                attackDirection = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                attackDirection = Vector2.right;
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                attackDirection = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                attackDirection = Vector2.down;
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
        if (Input.GetKeyDown(KeyCode.Space))  // Assuming space is the attack key
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

        //Call the attack animator
        AttackAnimation();

        // Create sword swing effect
        GameObject swordSwing = Instantiate(swordSwingPrefab, transform.position, Quaternion.identity);
        swordSwing.transform.up = -attackDirection;  // Set sword swing direction
        Destroy(swordSwing, attackDuration);

        // Perform attack logic (e.g., raycast to detect enemies)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // Handle enemy hit (e.g., apply damage)
            Debug.Log("Hit " + hit.collider.name);
        }

        yield return new WaitForSeconds(attackDuration);
    }

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
