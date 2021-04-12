using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float speed = 100.0f;
    public float jumpForce = 350.0f;
    public float airDrag = 0.8f;
    public Transform bottomTransform;

    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 currentVelocity;
    private float previousPositionY;

    private bool isOnGround;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
        HandleCollisions();
        previousPositionY = transform.position.y;
    }

    private void Move()
    {
        float velocity = Input.GetAxis("Horizontal") * speed;
        bool isJumping = Input.GetKey(KeyCode.Space);

        animator.SetFloat("Speed", Mathf.Abs(velocity));

        if (!isOnGround)
        {
            velocity *= airDrag;
        }

        // Horizontal Movement
        body.velocity = Vector2.SmoothDamp(body.velocity, new Vector2(velocity, body.velocity.y), ref currentVelocity, 0.02f);

        // Initiate Jump
        if (isOnGround && isJumping)
        {
            animator.SetBool("IsJumping", true);
            isOnGround = false;
            body.AddForce(new Vector2(0, jumpForce));
        }

        // Cancel Jump
        if (!isOnGround && !isJumping && body.velocity.y > 0.01f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.95f);
        }

        if (velocity < 0) spriteRenderer.flipX = true;
        else if (velocity > 0)
            spriteRenderer.flipX = false;
    }

    private void HandleCollisions()
    {
        bool wasOnGround = isOnGround;
        isOnGround = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(bottomTransform.position, 0.6f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                isOnGround = true;
                if (!wasOnGround && previousPositionY > transform.position.y)
                    HandleLanding();
            }
        }
    }

    private void HandleLanding()
    {
        animator.SetBool("IsJumping", false);
    }
}