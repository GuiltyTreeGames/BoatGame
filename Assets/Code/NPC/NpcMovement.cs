using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;

    private readonly List<Collider2D> _touchingColliders = new();

    [SerializeField]
    private float _moveSpeed = 1;

    [SerializeField]
    [Tooltip("NPC will move randomly if set to true")]
    private bool _hasRandomMovement = false;

    [SerializeField]
    [Min(1f)]
    [Tooltip("The minimum cooldown between two sessions of random movement")]
    private float _randomMovementMinimumInterval = 2f;

    [SerializeField]
    [Min(1f)]
    [Tooltip("The duration of each random movement session")]
    private float _randomMovementDuration = 2f;

    private bool _isRandomlyMoving = false;
    private float _randomMovementTimer = 0f;

    [Range(0f, 1f)]
    private float _randomMovementProbability = 0.5f;

    private float _changeDirectionMinimumInterval = 0.5f;
    private float _changeDirectionTimer = 0f;
    private float _changeDirectionProbability = 0.5f;


    /// <summary>
    /// Read-only, returns a random orthogonal movement direction (up, down, left, or right)
    /// </summary>
    public Vector2 RandomMovementDirection
    {
        get
        {
            float randomValue = UnityEngine.Random.value;
            switch (randomValue)
            {
                case > 0.75f:
                    return new Vector2(1, 0);
                case > 0.5f:
                    return new Vector2(0, 1);
                case > 0.25f:
                    return new Vector2(-1, 0);
                case >= 0f:
                    return new Vector2(0, -1);
                default:
                    return new Vector2(0, 0);
            }
        }
    }

    private Vector2 _randomMovementDirection = new Vector2(0, 0);

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        col = GetComponentInChildren<BoxCollider2D>();
        _randomMovementTimer = 0f;
        _isRandomlyMoving = false;
    }

    void FixedUpdate()
    {
        UpdateRandomMovementStatus();
        Vector2 direction = _isRandomlyMoving?
            KillBlockedMovement(_randomMovementDirection)
            : new Vector2(0, 0);
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

    private void UpdateRandomMovementStatus()
    {
        _randomMovementTimer += Time.fixedDeltaTime;

        if (!_isRandomlyMoving)
        {
            if (_randomMovementTimer > _randomMovementMinimumInterval)
            {
                if (UnityEngine.Random.value < _randomMovementProbability)
                {
                    // start random movement
                    _isRandomlyMoving = true;
                    _randomMovementDirection = RandomMovementDirection;
                    _randomMovementTimer = 0;
                }
            }
        }
        else if (_isRandomlyMoving)
        {
            _changeDirectionTimer += Time.fixedDeltaTime;
            if (_randomMovementTimer > _randomMovementDuration)
            {
                // stop random movement
                _isRandomlyMoving = false;
                _randomMovementTimer = 0;
                _changeDirectionTimer = 0;
            }
            if (_changeDirectionTimer > _changeDirectionMinimumInterval)
            {
                if (UnityEngine.Random.value < _changeDirectionProbability)
                {
                    // change movement direction
                    _randomMovementDirection = RandomMovementDirection;
                    _changeDirectionTimer = 0;
                }
            }
        }
    }
}
