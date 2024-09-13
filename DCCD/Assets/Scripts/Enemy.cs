using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerController player;

    public int maxHealth = 5; //Kinda self explanitory, no?
    public int Enemyhealth;
    public int damage = 1;
    public float speed = 3f;  // Speed of the enemy movement
    public int knockback = 100;



    private Rigidbody2D rb;  // Rigidbody component for physics interaction
    private bool isKnockedBack = false;
    private float knockbackDuration = 0.2f;  // Duration of the knockback effect
    


    // Start is called before the first frame update
    void Start()
    {
        Enemyhealth = maxHealth;
        
        // Find the player in the scene
        player = FindObjectOfType<PlayerController>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //He can die
        if (Enemyhealth <= 0)
        {
            player.money++;

            // Increment the enemy kill count
            PlayerController.enemyKillCount++;

            Destroy(gameObject);
        }
        //Enemy only moves when the player enters the same map where the enemy is
        if (!isKnockedBack && player.currentMap.tag == "Forest_1")
        {
            MoveTowardsPlayer(); // Call the movement method
        }
    }
    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            // Move the enemy towards the player
            transform.position += direction * speed * Time.deltaTime;
            //anim.SetFloat("Speed", 1);
        }
    }

    //The rest from here...If there isn't more than the knockback, is by gpt

    // Method to apply knockback to the enemy
    public void ApplyKnockback(Vector2 direction, float force)
    {
        isKnockedBack = true;

        // Apply the knockback force to the enemy's Rigidbody2D
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // Start the knockback recovery coroutine
        StartCoroutine(KnockbackRecovery());
    }

    // Coroutine to handle knockback recovery
    private IEnumerator KnockbackRecovery()
    {
        yield return new WaitForSeconds(knockbackDuration);

        // Stop the knockback and allow the enemy to move again
        isKnockedBack = false;
        rb.velocity = Vector2.zero;  // Reset the velocity to stop movement
    }
}
