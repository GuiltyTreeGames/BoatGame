using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoomOnTrigger : MonoBehaviour
{
    [SerializeField]
    private string _targetRoom;
    [SerializeField]
    private string _targetDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Core.RoomManager.ChangeRoom(_targetRoom);
    }
}
