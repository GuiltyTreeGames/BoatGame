using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    private RectTransform rect;

    [SerializeField]
    private float _dropSpeed = 1;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
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

    private const float MIN_POSITION = 10f;
}
