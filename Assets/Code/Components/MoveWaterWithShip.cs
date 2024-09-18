using UnityEngine;

public class MoveWaterWithShip : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 1;

    private float _startingHeight;

    private void Awake()
    {
        _startingHeight = transform.position.y;
    }

    private void Update()
    {
        transform.position += _movementSpeed * Time.deltaTime * Vector3.down;

        while (transform.position.y < _startingHeight - 1)
        {
            transform.position += Vector3.up;
        }
    }
}
