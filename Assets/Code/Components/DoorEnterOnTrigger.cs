using UnityEngine;

public class DoorEnterOnTrigger : MonoBehaviour
{
    [SerializeField]
    private Door door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            door.EnterDoor();
    }
}
