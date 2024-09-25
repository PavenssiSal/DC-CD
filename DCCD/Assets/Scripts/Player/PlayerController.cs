using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Enemy enemy;
    public YouLose loser;
    public QuestManager QuestManager;

    [Header("Map")]
    public GameObject initialMap;
    // Tracks the player's current map
    public GameObject currentMap;

    [Header("Smovement")]
    public bool canMove;
    public float speed = 2f;
    
    public float dashDistance = 5f;    // Distance covered during the dash
    public float dashDuration = 0.2f;  // Duration of the dash in seconds
    public float dashCooldown = 2f;    // Cooldown time between dashes

    private bool canDash = true;       // Indicates if the player can dash
    private bool isDashing = false;    // Tracks if the player is currently dashing

    [Header("Combat")]
    public float attackDuration = 0.5f;  // Duration of the attack animation
    public float attackRange = 3.0f;
    private bool canAttack = true;  // This will be used to check if the player can attack
    public float attackCooldown = 0.5f;  // Set the desired cooldown duration
    public int damage = 1;
    public int knockback = 100;
    public int MaxHealth = 5;
    public int currentHealth;
    public TMP_Text healthText;
    public TMP_Text MaxhealthText;
    private bool canTakeDamage = true;
    private float damadeCooldown = 0.5f; // How often the player can take damage
    public static int enemyKillCount = 0;
    public int tester;

    [Header("ANIMATOR")] //Animator stuff
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 movement;

    [Header("Everything else")] //Mischalanious stuff that doesn't need its own Header
    public int money;
    public TMP_Text coinText;

    private void Start()
    {
        loser = FindObjectOfType<YouLose>();
        Camera.main.GetComponent<MainCamera>().SetBound(initialMap);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;


        currentMap = initialMap;
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        tester = enemyKillCount;
        coinText.text = money.ToString();
        healthText.text = currentHealth.ToString();
        MaxhealthText.text = MaxHealth.ToString();

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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && canTakeDamage == true)
        {
            StartCoroutine(DamageEverything());  
        }
        else
        {
            
        }
    }
    private IEnumerator DamageEverything()
    {
        canTakeDamage = false; // Prevent further damage until the cooldown ends
        TakeDamage();
        yield return new WaitForSeconds(damadeCooldown); // Wait for the cooldown duration
        canTakeDamage = true;  // Allow damage again after the cooldown
    }
    private void TakeDamage()
    {
        currentHealth -= enemy.damage;

        if (currentHealth <= 0) //Jos el‰m‰t = 0, do death state ja pelaa ‰‰niefekti ja poistaa syd‰mmen, jonka takana on rikkin‰inen versio
        {
            
            //AudioManager.instance.Play("Death");

            //Heart.SetActive(false);

            currentHealth = 0;

            loser.TriggerGameOver();
        }
        else if (currentHealth > 0) //If hp more than 0, play damage ‰‰niefekti
        {
            //AudioManager.instance.Play("Damage");
            
        }

        //UpdateHealthText();
    }

    // Method to update the current map
    public void UpdateCurrentMap(GameObject newMap)
    {
        currentMap = newMap;
    }

    //They do something, even if transparent, dont delete
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
        if (!canAttack || !canMove || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield break;

        // Prevent further attacks until cooldown is over
        canAttack = false;

        // Set the attack animation trigger based on the direction the player last faced
        anim.SetBool("isAttack", true);

        // Perform the attack based on the last facing direction
        Vector2 attackPosition = rb.position;
        Vector2 attackDirection = new Vector2(anim.GetFloat("Last_Horizontal"), anim.GetFloat("Last_Vertical")).normalized;

        // Define the size of the attack box (a rectangular area in front of the player)
        Vector2 attackBoxSize = new Vector2(1.5f, 1.0f);  // Width and height of the attack box
        Vector2 attackBoxPosition = attackPosition + attackDirection * attackRange;  // Offset the box in front of the player

        // Visualize the attack area for debugging
    Debug.DrawLine(attackBoxPosition - attackBoxSize / 2, attackBoxPosition + attackBoxSize / 2, Color.red, attackDuration);

        // Use OverlapBoxAll to detect enemies in the area in front of the player
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackBoxPosition, attackBoxSize, 0f);



        // Process the hits, applying damage if necessary
        foreach (Collider2D hit in hitEnemies)
        {
            if (hit != null && hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                if (enemy != null)
                {
                    // Damage the enemy (assuming enemy has a TakeDamage method) will be added later
                    Debug.Log($"You hit something. Damage done " + damage);
                    enemy.Enemyhealth -= damage;

                    // Apply knockback
                    Vector2 knockbackDirection = (hit.transform.position - transform.position).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockback);
                }
                
            }  
        }

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(attackDuration);

        // Reset attack state
        anim.SetBool("isAttack", false);

        // Start cooldown period
        yield return new WaitForSeconds(attackCooldown);

        // Allow the player to attack again
        canAttack = true;
    }

    // Method to visualize the attack area
    private void DebugAttackBox(Vector2 center, Vector2 size)
    {
        // Draw the four edges of the rectangle
        Vector2 bottomLeft = center - size / 2;
        Vector2 topRight = center + size / 2;
        Vector2 topLeft = new Vector2(bottomLeft.x, topRight.y);
        Vector2 bottomRight = new Vector2(topRight.x, bottomLeft.y);

        Debug.DrawLine(bottomLeft, topLeft, Color.red, attackDuration);  // Left edge
        Debug.DrawLine(topLeft, topRight, Color.red, attackDuration);  // Top edge
        Debug.DrawLine(topRight, bottomRight, Color.red, attackDuration);  // Right edge
        Debug.DrawLine(bottomRight, bottomLeft, Color.red, attackDuration);  // Bottom edge
    }


    //Probably not atleast fully me
    private IEnumerator Dash(Vector2 direction)
    {
        if (direction.magnitude == 0)
            yield break;  // Prevent dashing with no direction
        canTakeDamage = false;
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
        canTakeDamage = true;
    }
}
