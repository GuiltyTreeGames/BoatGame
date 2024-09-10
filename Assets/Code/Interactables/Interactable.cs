using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject _outline;

    private bool _playerInRange;

    void Update()
    {
        _outline.SetActive(_playerInRange);

        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log($"Player interacted with {GetType().Name}");
            OnInteract();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player entered interactable range for {GetType().Name}");
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player left interactable range for {GetType().Name}");
            _playerInRange = false;
        }
    }

    protected abstract void OnInteract();
}
