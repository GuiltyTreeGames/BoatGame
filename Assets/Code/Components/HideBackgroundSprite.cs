using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBackgroundSprite : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer[] renderers;

    [SerializeField]
    private float _switchHeight;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float yDiff = player.transform.position.y - (transform.position.y + _switchHeight);
        
        foreach (var renderer in renderers)
        {
            renderer.sortingLayerName = yDiff <= 0 ? "Background" : "Foreground";
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
