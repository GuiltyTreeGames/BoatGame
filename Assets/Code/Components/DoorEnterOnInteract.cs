using UnityEngine;

public class DoorEnterOnInteract : Interactable
{
    [SerializeField]
    private Door door;

    protected override void OnInteract()
    {
        door.EnterDoor();
    }
}
