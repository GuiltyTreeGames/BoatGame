using UnityEngine;
using UnityEngine.EventSystems;

public class Wheel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private float _maxAngle = 120;

    private Vector2 _lastPosition;
    private bool _lastDragging = false;
    private float _maxDistance;

    void Awake()
    {
        _maxDistance = GetComponent<CircleCollider2D>().radius / 2;
        Debug.Log("Max distance: " + _maxDistance);
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        if (_lastDragging)
            HandleRotation(eventData.position);

        _lastPosition = eventData.position;
        _lastDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _lastDragging = false;
    }

    private void HandleRotation(Vector2 currentPosition)
    {
        Vector3 a = transform.position - (Vector3)_lastPosition;
        Vector3 b = new Vector3(a.x, a.y, -1);

        float speedPercent = Mathf.Clamp(a.magnitude, 0, _maxDistance) / _maxDistance;

        Vector3 cross = Vector3.Cross(a, b).normalized * speedPercent;

        Vector3 l = (currentPosition - _lastPosition).normalized;
        Vector3 r = cross;

        float dot = -Vector3.Dot(l, r);

        float angle = transform.localEulerAngles.z;
        if (dot > 0 && (angle >= 180 || angle < _maxAngle) ||
            dot < 0 && (angle <= 180 || angle > 360 - _maxAngle))
        {
            transform.Rotate(new Vector3(0, 0, dot));
        }

        DEBUG_P1 = transform.position;
        DEBUG_P2 = _lastPosition;
        DEBUG_P3 = _lastPosition + (Vector2)cross * 100;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(DEBUG_P1, DEBUG_P2);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(DEBUG_P2, DEBUG_P3);
    }

    private Vector2 DEBUG_P1;
    private Vector2 DEBUG_P2;
    private Vector2 DEBUG_P3;
}
