using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStats playerStats; // Reference to the player's stats
    public bool IsInvincible { get; set; } = false; // To keep track of invincibility

    private Rigidbody2D rb;

    [HideInInspector] public Vector2 moveDir;
    [HideInInspector] public float lastHorizontalVector;
    [HideInInspector] public float lastVerticalVector;
    [HideInInspector] public Vector2 lastMovedVector;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); // If we don't do this and game starts up and don't move, the projectile weapon will have no momentum
    }

    void Update()
    {
        InputManagement();
    }

    void FixedUpdate() // Always calculate physics in fixed update
    {
        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized; // Use normalize as moving in diagonal generates a value > 1 so cap it to 1

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f); // Last moved X
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector); // Last moved Y
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector); // While moving
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * playerStats.MoveSpeed, moveDir.y * playerStats.MoveSpeed); // Apply velocity
    }

    public void TakeDamage(float damage)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(damage);
        }
    }
}
