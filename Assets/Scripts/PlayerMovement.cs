using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // The movement speed of the player
    [SerializeField] private float jumpForce = 30f; // The force applied when the player jumps
    [SerializeField] private LayerMask groundLayer; // The layer mask for detecting the ground
    private Rigidbody2D rb2; // Reference to the Rigidbody2D component
    private SpriteRenderer sprite; // Reference to the SpriteRenderer component
    private Animator anim; // Reference to the Animator component
    private BoxCollider2D boxCollider; // Reference to the BoxCollider2D component

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        sprite = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        anim = GetComponent<Animator>(); // Get the Animator component
        boxCollider = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // Get the horizontal input axis

        MovePlayer(moveX); // Move the player
        FlipCharacter(moveX); // Flip the character sprite based on the movement direction
        UpdateAnimation(moveX); // Update the animation states
        print(OnWall()); // Print if the player is on a wall
    }

    private void MovePlayer(float moveX)
    {
        rb2.velocity = new Vector2(moveX * speed, rb2.velocity.y); // Move the player horizontally

        if (Input.GetButtonDown("Jump") && IsGrounded()) // Check if the jump button is pressed and the player is grounded
        {
            rb2.velocity = new Vector2(rb2.velocity.x, jumpForce); // Apply vertical force to make the player jump
        }
    }

    private void FlipCharacter(float moveX)
    {
        if (moveX < -0.1f) // If the player is moving left
        {
            sprite.flipX = true; // Flip the character sprite horizontally
        }
        else if (moveX > 0.1f) // If the player is moving right
        {
            sprite.flipX = false; // Reset the character sprite orientation
        }
    }

    private void UpdateAnimation(float moveX)
    {
        anim.SetBool("isRun", moveX != 0); // Set the "isRun" parameter in the animator based on the movement
        anim.SetBool("isJump", !IsGrounded()); // Set the "isJump" parameter in the animator based on the grounded state
        anim.SetBool("isFall", rb2.velocity.y < 0 && !IsGrounded()); // Set the "isFall" parameter in the animator based on the falling state
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer); // Cast a box-shaped raycast downwards to check if the player is grounded
        return raycastHit.collider != null; // Return true if the raycast hits a collider, indicating the player is grounded
    }

    private bool OnWall()
    {
        RaycastHit2D leftRaycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.x + 0.1f, groundLayer); // Cast a raycast to the left to check if the player is on a wall
        RaycastHit2D rightRaycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.x + 0.1f, groundLayer); // Cast a raycast to the right to check if the player is on a wall

        return (leftRaycastHit.collider != null && !IsGrounded()) || (rightRaycastHit.collider != null && !IsGrounded()); // Return true if either the left or right raycast hits a collider and the player is not grounded, indicating the player is on a wall
    }
}
