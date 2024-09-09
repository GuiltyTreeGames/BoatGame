using System.Linq;
using UnityEngine;

public class SpawnManager : BaseManager
{
    private GameObject _playerPrefab;

    private string _nextSpawnId;
    private Vector2 _storedPosition;

    public GameObject SpawnedPlayer { get; private set; }

    public override void OnInitialize()
    {
        _playerPrefab = Resources.Load<GameObject>("Player");

        Core.RoomManager.OnRoomLoaded += OnRoomLoaded;
        Core.RoomManager.OnRoomUnloaded += OnRoomUnloaded;
    }

    public override void OnDispose()
    {
        Core.RoomManager.OnRoomLoaded -= OnRoomLoaded;
        Core.RoomManager.OnRoomUnloaded -= OnRoomUnloaded;
    }

    public void StorePlayerInfo(string targetId)
    {
        _nextSpawnId = targetId;
        _storedPosition = SpawnedPlayer.transform.position;
        // Store orientation
    }

    private void SpawnPlayer()
    {
        SpawnInfo spawn = GetSpawnDoorInfo(_nextSpawnId);

        SpawnedPlayer = Object.Instantiate(_playerPrefab);
        SpawnedPlayer.transform.position = GetSpawnPosition(spawn, _storedPosition);
        // Set orientation
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

    private void OnRoomLoaded(string room)
    {
        SpawnPlayer();
    }

    private void OnRoomUnloaded(string room)
    {
        SpawnedPlayer = null;
    }
}
