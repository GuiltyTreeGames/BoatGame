using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class Core
{
    private static bool _initialized;
    private static readonly List<BaseManager> _managers = new();

    [RuntimeInitializeOnLoadMethod]
    static void Startup()
    {
        if (_initialized)
            return;

        LoadManagers();
        Application.quitting += Shutdown;

        Debug.Log("Initializing core managers");
        foreach (var manager in _managers)
            manager.OnInitialize();
        foreach (var manager in _managers)
            manager.OnAllInitialized();
        Debug.Log("Completed initialization of all core managers");

        Debug.Log("Initialize time handler");
        GameObject timeHandlerObject = new("Time Handler");
        timeHandlerObject.AddComponent<TimeHandler>();
        Core.TimeHandler = timeHandlerObject.GetComponent<TimeHandler>();
        UnityEngine.Object.DontDestroyOnLoad(timeHandlerObject);
        Debug.Log("Completed Initialization of time handler");

        _initialized = true;
    }

    static void Shutdown()
    {
        if (!_initialized)
            return;

        Debug.Log("Disposing core managers");
        foreach (var manager in _managers)
            manager.OnDispose();
        Debug.Log("Disposed all core managers");
    }

    static void LoadManagers()
    {
        _managers.AddRange(typeof(Core).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
            .Where(x => x.PropertyType.IsSubclassOf(typeof(BaseManager)))
            .Select(x => x.GetValue(null) as BaseManager));
    }

    public static CanvasManager CanvasManager { get; private set; } = new();
    public static InputManager InputManager { get; private set; } = new();
    public static RoomManager RoomManager { get; private set; } = new();
    public static SpawnManager SpawnManager { get; private set; } = new();

    public static NpcManager NpcManager { get; private set; } = new();

    public static TimeHandler TimeHandler { get; private set; }
    
}
