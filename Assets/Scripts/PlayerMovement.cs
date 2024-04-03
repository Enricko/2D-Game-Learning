using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 30f;
    private Rigidbody2D rb2;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool isGrounded;

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        MovePlayer(moveX);
        FlipCharacter(moveX);
        UpdateAnimation(moveX);
    }

    private void MovePlayer(float moveX)
    {
        rb2.velocity = new Vector2(moveX * speed, rb2.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2.velocity = new Vector2(rb2.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void FlipCharacter(float moveX)
    {
        sprite.flipX = moveX < 0;
    }

    private void UpdateAnimation(float moveX)
    {
        anim.SetBool("isRun", moveX != 0);
        anim.SetBool("isJump", !isGrounded);
        anim.SetBool("isFall", rb2.velocity.y < 0 && !isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
