using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public TimeHandler() { }

    private readonly Dictionary<string, ITimeable> _timers = new();


    public void AddTimeable(string id, ITimeable timeable)
    {
        if (_timers.ContainsKey(id))
        {
            Debug.Log($"A timer with id {id} already exists.");
            return;
        }
        Debug.Log($"Adding timer: {id}");
        _timers.Add(id, timeable);
    }

    public void RemoveTimeable(string id)
    {
        Debug.Log($"Removing timer: {id}");
        _timers.Remove(id);
    }

    private void RemoveTimeables(Func<ITimeable, bool> predicate)
    {
        var toRemove = _timers.Where(x => predicate(x.Value)).ToArray();
        foreach (var t in toRemove)
        {
            Debug.Log($"Stopping timer: {t.Key}");
            _timers.Remove(t.Key);
        }
    }

    public bool HasTimeable(string id)
    {
        return _timers.ContainsKey(id);
    }


    public void Update()
    {
        if (!Core.RoomManager.GameSceneLoaded)
            return;

        // Update each timer
        foreach (var timer in _timers.Values.ToList())
            timer.OnUpdate();

        // Remove all unecessary timers
        RemoveTimeables(x => x.ShouldBeRemoved);
    }

    public void Reset()
    {
        RemoveTimeables(x => !x.IsPermanent);
    }
}
