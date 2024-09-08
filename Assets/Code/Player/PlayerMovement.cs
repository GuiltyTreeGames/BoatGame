using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private PlayerInput input;

    private readonly List<Collider2D> _touchingColliders = new();

    [SerializeField]
    private float _moveSpeed = 1;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        col = GetComponentInChildren<BoxCollider2D>();
        input = GetComponentInChildren<PlayerInput>();
    }

    void FixedUpdate()
    {
        Vector2 direction = KillBlockedMovement(input.MovementDirection);
        rb.velocity = _moveSpeed * Time.fixedDeltaTime * direction.normalized;
    }

    /// <summary>
    /// For every collider that the player is in contact with, kill movement in that direction to prevent slowing down
    /// </summary>
    private Vector2 KillBlockedMovement(Vector2 direction)
    {
        foreach (Collider2D collider in _touchingColliders)
        {
            Bounds playerBounds = col.bounds;
            Bounds colBounds = collider.bounds;

            if (direction.x > 0 && colBounds.min.x >= playerBounds.max.x ||
                direction.x < 0 && colBounds.max.x <= playerBounds.min.x)
            {
                direction.x = 0;
                Debug.Log("Killing horizontal movement");
            }
            if (direction.y > 0 && colBounds.min.y >= playerBounds.max.y ||
                direction.y < 0 && colBounds.max.y <= playerBounds.min.y)
            {
                direction.y = 0;
                Debug.Log("Killing vertical movement");
            }
        }

        return direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entering collider: " + collision.gameObject.name);
        _touchingColliders.Add(collision.collider);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exiting collider: " + collision.gameObject.name);
        _touchingColliders.Remove(collision.collider);
    }
}
