using System.Linq;
using UnityEngine;

public class SpawnManager : BaseManager
{
    private string _nextSpawnId;
    private Vector2 _storedPosition;

    public GameObject PlayerObject { get; private set; }

    public override void OnInitialize()
    {
        Core.NewRoomManager.OnGameplayLoaded += OnGameplayLoaded;
        Core.NewRoomManager.OnRoomLoaded += OnRoomLoaded;
    }

    public override void OnDispose()
    {
        Core.NewRoomManager.OnGameplayLoaded -= OnGameplayLoaded;
        Core.NewRoomManager.OnRoomLoaded -= OnRoomLoaded;
    }

    private void OnGameplayLoaded()
    {
        Debug.Log("Caching player object");
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnRoomLoaded(string _)
    {
        SpawnInfo spawn = GetSpawnDoorInfo(_nextSpawnId);

        PlayerObject.transform.position = GetSpawnPosition(spawn, _storedPosition);
        // Set orientation
    }

    public void StorePlayerInfo(string targetId)
    {
        _nextSpawnId = targetId;
        _storedPosition = PlayerObject.transform.position;
        // Store orientation
    }

    private SpawnInfo GetSpawnDoorInfo(string spawnId)
    {
        var doors = Object.FindObjectsOfType<Door>();

        Door targetDoor = doors.FirstOrDefault(x => x.DoorId == spawnId);
        if (targetDoor != null)
        {
            Debug.Log($"Spawning player at door {spawnId}");
            return targetDoor.DoorSpawn;
        }

        Door firstDoor = doors.FirstOrDefault();
        if (firstDoor != null)
        {
            Debug.LogWarning($"Door with id {spawnId} not found.  Spawning player at first door instead");
            return firstDoor.DoorSpawn;
        }

        Debug.LogWarning($"No doors found in the scene.  Spawning player at origin instead");
        return new SpawnInfo()
        {
            customPositionX = 0,
            customPositionY = 0
        };
    }

    private Vector3 GetSpawnPosition(SpawnInfo spawn, Vector2 storedPosition)
    {
        if (!spawn.preservePositionX)
            storedPosition.x = spawn.customPositionX;
        if (!spawn.preservePositionY)
            storedPosition.y = spawn.customPositionY;
        return storedPosition;
    }
}
