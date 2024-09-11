using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Camera cam;

    [SerializeField]
    float _damping;
    [SerializeField]
    private Vector2 _xBounds;
    [SerializeField]
    private Vector2 _yBounds;

    private Vector3 _lastUnsmoothPosition;

    public void UpdateBounds(Vector2 x, Vector2 y)
    {
        _xBounds = x;
        _yBounds = y;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        transform.position = CalculateTargetPosition();
        _lastUnsmoothPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = CalculateTargetPosition();
        Vector3 lerpedPosition = Vector3.Lerp(_lastUnsmoothPosition, targetPosition, _damping * Time.deltaTime);
        Vector3 smoothedPosition = SmoothPosition(lerpedPosition);

        transform.position = smoothedPosition;
        _lastUnsmoothPosition = lerpedPosition;
    }

    private Vector3 CalculateTargetPosition()
    {
        Vector3 targetPosition = Core.SpawnManager.SpawnedPlayer.transform.position;
        targetPosition.z = -10;

        // x bounds
        if (_xBounds.x == _xBounds.y)
        {
            targetPosition.x = _xBounds.x;
        }
        else
        {
            if (targetPosition.x + Xsize > _xBounds.y)
                targetPosition.x = _xBounds.y - Xsize;
            else if (targetPosition.x - Xsize < _xBounds.x)
                targetPosition.x = _xBounds.x + Xsize;
        }

        // y bounds
        if (_yBounds.x == _yBounds.y)
        {
            targetPosition.y = _yBounds.x;
        }
        else
        {
            if (targetPosition.y + Ysize > _yBounds.y)
                targetPosition.y = _yBounds.y - Ysize;
            else if (targetPosition.y - Ysize < _yBounds.x)
                targetPosition.y = _yBounds.x + Ysize;
        }

        return targetPosition;
    }

    private Vector3 SmoothPosition(Vector3 position)
    {
        return new Vector3(((int)(position.x * 32)) / 32f, ((int)(position.y * 32)) / 32f, -10);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(_xBounds.x, _yBounds.x), new Vector2(_xBounds.x, _yBounds.y));
        Gizmos.DrawLine(new Vector2(_xBounds.x, _yBounds.y), new Vector2(_xBounds.y, _yBounds.y));
        Gizmos.DrawLine(new Vector2(_xBounds.y, _yBounds.y), new Vector2(_xBounds.y, _yBounds.x));
        Gizmos.DrawLine(new Vector2(_xBounds.y, _yBounds.x), new Vector2(_xBounds.x, _yBounds.x));
    }

    private float Xsize => cam.orthographicSize * cam.aspect;
    private float Ysize => cam.orthographicSize;
}
