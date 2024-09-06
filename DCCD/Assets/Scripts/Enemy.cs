using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Warp warp;
    private PlayerController player;
    public int maxHealth = 3; //Kinda self explanitory, no?
    public int Enemyhealth;
    public float speed = 2f;  // Speed of the enemy movement

    [Header("ANIMATOR")] //Animator stuff
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 movement;


    // Start is called before the first frame update
    void Start()
    {
        Enemyhealth = maxHealth;

        
        // Find the player in the scene
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemyhealth <= 0)
        {
            player.money++;
            Destroy(gameObject);
        }

        if (player.currentMap.tag == "Forest_1")
        {
            MoveTowardsPlayer(); // Call the movement method
        }
        else
        {

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
}
