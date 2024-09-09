using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //SceneManager.sceneLoaded += SceneLoaded;
        //SceneManager.sceneUnloaded += SceneUnloaded;

        Debug.Log("Initializing core managers");
        foreach (var manager in _managers)
            manager.OnInitialize();
        foreach (var manager in _managers)
            manager.OnAllInitialized();
        Debug.Log("Initialized all core managers");

        _initialized = true;
    }

    //static void SceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name == "MainMenu")
    //        return;

    //    Debug.Log($"Loaded scene {scene.name}");
    //    foreach (var manager in _managers)
    //        manager.OnRoomLoaded(scene.name);
    //}

    //static void SceneUnloaded(Scene scene)
    //{
    //    if (scene.name == "MainMenu")
    //        return;

    //    Debug.Log($"Unloaded scene {scene.name}");
    //    foreach (var manager in _managers)
    //        manager.OnRoomUnloaded(scene.name);
    //}

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
    public static RoomManager RoomManager { get; private set; } = new();
    public static SpawnManager SpawnManager { get; private set; } = new();
}
