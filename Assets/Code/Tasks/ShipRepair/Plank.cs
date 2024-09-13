using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plank : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rect;

    [SerializeField]
    private float _dropSpeed = 1;

    private bool _isDragging = false;
    private Vector2 _dragOffset;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_isDragging)
            return;

        // If floating, apply gravity
        if (rect.anchoredPosition.y > MIN_POSITION)
        {
            rect.anchoredPosition += _dropSpeed * Time.deltaTime * Vector2.down;
        }

        // If below bottom, snap it to bottom
        if (rect.anchoredPosition.y < MIN_POSITION)
        {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, MIN_POSITION);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.position);

        float x = Mathf.Clamp(eventData.position.x, MIN_POSITION, Screen.width - MIN_POSITION);
        float y = Mathf.Clamp(eventData.position.y, MIN_POSITION, Screen.height - MIN_POSITION);

        transform.position = _dragOffset + new Vector2(x, y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        _dragOffset = (Vector2)transform.position - eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
    }

    private const float MIN_POSITION = 10f;
}
