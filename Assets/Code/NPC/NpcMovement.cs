using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private BoxCollider2D thisCollider;

    private List<Collider2D> _touchingColliders = new();

    [SerializeField]
    private float _moveSpeed = 1;

    [SerializeField]
    [Tooltip("NPC will move randomly if set to true")]
    private bool _hasRandomMovement = false;

    [SerializeField]
    [Tooltip("The min and max possible cooldown between two sessions of random movement")]
    private Vector2 _randomMovementIntervalRange = new(2f, 4f);

    [SerializeField]
    [Tooltip("The possible range of duration of each random movement session")]
    private Vector2 _randomMovementDurationRange = new(2f, 4f);

    [SerializeField]
    [Tooltip("The X boundary of random movement")]
    private Vector2 _movementBoundsX;

    [SerializeField]
    [Tooltip("The Y boundary of random movement")]
    private Vector2 _movementBoundsY;

    private Vector2 _randomMovementTurnIntervalRange = new(0.5f, 1f);

    private bool _isRandomlyMoving = false;


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
        rigidBody = GetComponent<Rigidbody2D>();
        thisCollider = transform.Find("Collision").GetComponentInChildren<BoxCollider2D>();

    }


    void FixedUpdate()
    {
        if (!_isRandomlyMoving 
            && !Core.TimeHandler.HasTimeable($"{gameObject.name}_NpcRandomMovement_WaitAndStart"))
        {
            Core.TimeHandler.AddTimeable(
                $"{gameObject.name}_NpcRandomMovement_WaitAndStart",
                new RandomTicker(_randomMovementIntervalRange, false, StartRandomMovement));
        }
        Vector2 direction = _isRandomlyMoving
            ? KillBlockedMovement(_randomMovementDirection)
            : new Vector2(0, 0);
        rigidBody.velocity = _moveSpeed * Time.fixedDeltaTime * direction.normalized;
    }

    /// <summary>
    /// For every collider that the NPC is in contact with, kill movement in that direction to prevent slowing down
    /// </summary>
    private Vector2 KillBlockedMovement(Vector2 direction)
    {
        Bounds npcBounds = this.thisCollider.bounds;

        foreach (Collider2D collider in _touchingColliders)
        {
            Bounds colBounds = collider.bounds;

            if (direction.x > 0 && colBounds.min.x >= npcBounds.max.x ||
                direction.x < 0 && colBounds.max.x <= npcBounds.min.x)
            {
                direction.x = 0;
                Debug.Log($"Killing horizontal movement of {gameObject}");
            }
            if (direction.y > 0 && colBounds.min.y >= npcBounds.max.y ||
                direction.y < 0 && colBounds.max.y <= npcBounds.min.y)
            {
                direction.y = 0;
                Debug.Log($"Killing vertical movement of {gameObject}");
            }
        }

        // kill movement if the NPC is about to approach its movementBounds
        if (direction.x > 0 && _movementBoundsX.y <= npcBounds.max.x ||
            direction.x < 0 && _movementBoundsX.x >= npcBounds.min.x)
        {
            direction.x = 0;
            Debug.Log($"Killing horizontal movement of {gameObject} when touching Npc Movement Bounds");
        }
        if (direction.y > 0 && _movementBoundsY.y <= npcBounds.max.y ||
            direction.y < 0 && _movementBoundsY.x >= npcBounds.min.y)
        {
            direction.y = 0;
            Debug.Log($"Killing vertical movement of {gameObject} when touching Npc Movement Bounds");
        }

        return direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} entering collider: {collision.gameObject.name}");

        if (collision.gameObject.name == "MovementBounds"
            && collision.gameObject.transform.parent.gameObject.name == this.gameObject.name)
        {
            return;
        }
        else
        {
            _touchingColliders.Add(collision.collider);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} exiting collider: {collision.gameObject.name}");

        if (collision.gameObject.name == "MovementBounds"
            && collision.gameObject.transform.parent.gameObject.name == this.gameObject.name)
        {
            return;
        }
        else
        {
            _touchingColliders.Remove(collision.collider);
        }
    }


    private void StartRandomMovement()
    {
        _isRandomlyMoving = true;
        Core.TimeHandler.AddTimeable(
            $"{gameObject.name}_NpcRandomMovement_WaitAndTurn",
            new RandomTicker(_randomMovementTurnIntervalRange, false, ChangeRandomMovementDirection));
        Core.TimeHandler.AddTimeable(
            $"{gameObject.name}_NpcRandomMovement_WaitAndEnd",
            new RandomCountdown(_randomMovementDurationRange, StopRandomMovement));
        ChangeRandomMovementDirection();
    }

    private void StopRandomMovement()
    {
        _isRandomlyMoving = false;
        Core.TimeHandler.RemoveTimeable($"{gameObject.name}_NpcRandomMovement_WaitAndTurn");
    }

    private void ChangeRandomMovementDirection()
    {
        _randomMovementDirection = RandomMovementDirection;
    }
}
