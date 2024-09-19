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
        Core.NewRoomManager.ChangeRoom(_targetRoom);
    }

    private void OnDrawGizmosSelected()
    {
        float x = _spawn.preservePositionX ? transform.position.x : _spawn.customPositionX;
        float y = _spawn.preservePositionY ? transform.position.y : _spawn.customPositionY;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(x, y), 0.2f);
    }
}
