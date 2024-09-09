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

    public void EnterDoor()
    {
        // Set spawn properties

        Core.RoomManager.ChangeRoom(_targetRoom);
    }

    public SpawnInfo GetSpawnInfo()
    {
        return _spawn;
    }
}
