using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float _moveSpeed = 1;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y).normalized;

        // Set physics velocity
        rb.velocity = _moveSpeed * Time.fixedDeltaTime * direction;
        Debug.Log(rb.velocity);
    }
}
