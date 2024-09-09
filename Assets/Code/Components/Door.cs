using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private string _id;
    [SerializeField]
    private string _targetRoom;
    [SerializeField]
    private string _targetId;
    [SerializeField]
    private SpawnInfo _spawn;

    public string DoorId => _id;
    public SpawnInfo DoorSpawn => _spawn;

    public void EnterDoor()
    {
        Core.SpawnManager.StorePlayerInfo(_targetId);
        Core.RoomManager.ChangeRoom(_targetRoom);
    }
}
