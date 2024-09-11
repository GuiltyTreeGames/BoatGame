using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : BaseManager
{
    private string _nextSpawnId;
    private Vector2 _storedPlayerPosition;


    private GameObject _playerPrefab;
    public GameObject SpawnedPlayer { get; private set; }
    public Dictionary<string, GameObject> SpawnedNpcs { get; private set; } = new();

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
        _storedPlayerPosition = SpawnedPlayer.transform.position;
        // Store orientation
    }

    private void SpawnPlayer()
    {
        SpawnInfo spawn = GetSpawnDoorInfo(_nextSpawnId);

        SpawnedPlayer = Object.Instantiate(_playerPrefab);
        SpawnedPlayer.transform.position = GetSpawnPosition(spawn, _storedPlayerPosition);
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

    private void SpawnNpc(string name)
    {
        GameObject npc = Core.NpcManager.npcPrefabs[name];
        NpcInfo npcInfo = npc.GetComponent<NpcInfo>();

        SpawnInfo spawn = npcInfo.spawnInfo;
        SpawnedNpcs.Add(name, Object.Instantiate(npc));
        SpawnedNpcs[name].transform.position = new Vector3(spawn.customPositionX, spawn.customPositionY, 0);
        // Set orientation
    }

    private void SpawnAllLivingNpcs()
    {
        foreach (GameObject npc in Core.NpcManager.npcPrefabs.Values.Where(x => x.GetComponent<NpcInfo>().isDead == false))
        {
            SpawnNpc(npc.name);
        }
    }

    private void OnRoomLoaded(string room)
    {
        if (room == "MainMenu")
            return;

        SpawnPlayer();
        SpawnAllLivingNpcs();
    }

    private void OnRoomUnloaded(string room)
    {
        //SpawnedNpcs = null;
        //SpawnedPlayer = null;
    }
}
