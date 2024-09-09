using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Core
{
    private static bool _initialized;
    private static readonly List<BaseManager> _managers = new();

    static Core()
    {

    }

    [RuntimeInitializeOnLoadMethod]
    static void Startup()
    {
        if (_initialized)
            return;

        Debug.Log("Initializing core managers");
        foreach (var manager in _managers)
            manager.OnInitialize();
        foreach (var manager in _managers)
            manager.OnAllInitialized();
        Debug.Log("Initialized all core managers");

        Application.quitting += Shutdown;
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
}
