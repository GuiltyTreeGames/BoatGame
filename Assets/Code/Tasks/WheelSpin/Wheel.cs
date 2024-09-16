using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wheel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private CircleCollider2D col;

    private Vector2 _lastPosition;
    private bool _lastDragging = false;
    private float _maxDistance;

    void Awake()
    {
        _maxDistance = Mathf.Pow(GetComponent<CircleCollider2D>().radius / 2, 2);
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
        Vector2 centerToLast = _lastPosition - (Vector2)transform.position;

        Debug.LogWarning($"Sqr distance from center: {centerToLast.sqrMagnitude}");

        Vector3 a = transform.position - (Vector3)_lastPosition;
        Vector3 b = new Vector3(a.x, a.y, -1);

        float speedPercent = Mathf.Clamp(a.sqrMagnitude, 0, _maxDistance) / _maxDistance;
        Debug.LogWarning($"Speed percent: {speedPercent}");

        Vector3 side = Vector3.Cross(a, b);
        side = side.normalized * 100 * speedPercent;

        DEBUG_P1 = transform.position;
        DEBUG_P2 = _lastPosition;
        DEBUG_P3 = _lastPosition + (Vector2)side;
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
