using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;                // Normal movement speed
        public float dashDistance = 5f;    // Distance covered during the dash
        public float dashDuration = 0.2f;  // Duration of the dash in seconds
        public float dashCooldown = 2f;    // Cooldown time between dashes

        private bool canDash = true;       // Indicates if the player can dash
        private bool isDashing = false;    // Tracks if the player is currently dashing
        private Animator animator;
        private Rigidbody2D rb;

        private void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 dir = Vector2.zero;

            // Movement Input
            if (!isDashing) // Disable normal movement during dash
            {
                if (Input.GetKey(KeyCode.A))
                {
                    dir.x = -1;
                    animator.SetInteger("Direction", 3);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    dir.x = 1;
                    animator.SetInteger("Direction", 2);
                }

                if (Input.GetKey(KeyCode.W))
                {
                    dir.y = 1;
                    animator.SetInteger("Direction", 1);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    dir.y = -1;
                    animator.SetInteger("Direction", 0);
                }

                dir.Normalize();
                animator.SetBool("IsMoving", dir.magnitude > 0);

                // Normal movement
                rb.velocity = speed * dir;
            }

            // Dash input
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
            {
                StartCoroutine(Dash(dir));
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
}
