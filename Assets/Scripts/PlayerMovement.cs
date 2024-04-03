using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rb2;
    private SpriteRenderer sprite;
    private Animator anim;

    public void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        rb2.velocity = new Vector2(moveX * speed, rb2.velocity.y);

        FlipChar(moveX);

        Jump();
        MoveAnimation(moveX);
    }
    public void Jump()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            rb2.velocity = new Vector2(rb2.velocity.x, speed);
        }
    }

    public void FlipChar(float moveX)
    {
        if (moveX < 0)
            sprite.flipX = true;
        else if (moveX > 0)
            sprite.flipX = false;
    }
    public void MoveAnimation(float moveX)
    {
        anim.SetBool("run", moveX != 0);
    }
}
