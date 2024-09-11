
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : BaseManager
{
    internal Dictionary<string, GameObject> npcPrefabs = new();

    public override void OnInitialize()
    {
        List<GameObject> npcList = new(Resources.LoadAll<GameObject>("NPC"));
        foreach (GameObject npc in npcList)
        {
            npcPrefabs.Add(npc.name, npc);
        }

        Core.RoomManager.OnRoomLoaded += OnRoomLoaded;
        Core.RoomManager.OnRoomUnloaded += OnRoomUnloaded;
    }

    private void OnRoomLoaded(string room)
    {
    }

    private void OnRoomUnloaded(string room)
    {
    }
}