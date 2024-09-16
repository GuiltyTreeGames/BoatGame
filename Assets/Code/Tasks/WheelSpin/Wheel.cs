using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Wheel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 _lastPosition;
    private bool _lastDragging = false;

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

    }
}
