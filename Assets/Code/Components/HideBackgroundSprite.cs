using UnityEngine;

public class HideBackgroundSprite : MonoBehaviour
{
    private SpriteRenderer[] renderers;

    private bool _playerHidden;

    [SerializeField]
    private float _switchHeight;

    void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float yDiff = Core.SpawnManager.SpawnedPlayer.transform.position.y - (transform.position.y + _switchHeight);

        foreach (var renderer in renderers)
        {
            renderer.sortingLayerName = yDiff <= 0 ? "Background" : "Foreground";
            renderer.color = SetColorAlpha(renderer.color, _playerHidden ? 0.5f : 1);
        }
    }

    private Color SetColorAlpha(Color color, float value)
    {
        color.a = value;
        return color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerHidden = true;
            Debug.Log("Making backgrounds sprite transparent");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerHidden = false;
            Debug.Log("Making backgrounds sprite opaque");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Vector3 left = new Vector3(transform.position.x - 3, transform.position.y + _switchHeight);
        Vector3 right = new Vector3(transform.position.x + 3, transform.position.y + _switchHeight);
        Gizmos.DrawLine(left, right);
    }
}
