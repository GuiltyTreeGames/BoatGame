using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    [SerializeField]
    private Vector2 _xBounds;
    [SerializeField]
    private Vector2 _yBounds;

    private void Start()
    {
        Debug.Log("Updating camera bounds");
        Camera.main.GetComponent<CameraFollowPlayer>().UpdateBounds(_xBounds, _yBounds);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(_xBounds.x, _yBounds.x), new Vector2(_xBounds.x, _yBounds.y));
        Gizmos.DrawLine(new Vector2(_xBounds.x, _yBounds.y), new Vector2(_xBounds.y, _yBounds.y));
        Gizmos.DrawLine(new Vector2(_xBounds.y, _yBounds.y), new Vector2(_xBounds.y, _yBounds.x));
        Gizmos.DrawLine(new Vector2(_xBounds.y, _yBounds.x), new Vector2(_xBounds.x, _yBounds.x));
    }
}
