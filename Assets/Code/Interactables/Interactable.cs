using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject _outline;

    private bool _playerInRange;

    void Update()
    {
        _outline.SetActive(_playerInRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player entered interactable range for {name}");
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player left interactable range for {name}");
            _playerInRange = false;
        }
    }
}
