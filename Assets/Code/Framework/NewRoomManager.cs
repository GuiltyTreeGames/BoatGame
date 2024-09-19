using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewRoomManager : BaseManager
{
    public string CurrentRoom { get; private set; } = string.Empty;
    private string _nextRoom = string.Empty;

    private string _loadRoom = string.Empty;
    private Action<string> _loadCallback = null;
    private int _loadCounter;

    private string _unloadRoom = string.Empty;
    private Action<string> _unloadCallback = null;
    private int _unloadCounter;

    public override void OnAllInitialized()
    {
        Debug.Log($"Starting in {SceneManager.GetActiveScene().path}");

        Scene startingScene = SceneManager.GetActiveScene();
        if (startingScene.name == "MainMenu")
        {
            SendMainMenuLoadedEvent();
        }
        else if (startingScene.name == "Gameplay")
        {
            SendGameplayLoadedEvent();
        }
        else if (startingScene.path.Contains(GAMEPLAY_SCENE_PATH))
        {
            // When pressing play from a room scene
            string room = Path.GetFileName(Path.GetDirectoryName(startingScene.path));
            LoadGameplay(room);
        }
    }

    // Loading menu

    public void LoadMainMenu()
    {
        UnloadAllScenes(CurrentRoom, UnloadRoomBeforeMainMenu);
    }

    private void UnloadRoomBeforeMainMenu(string room)
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLoadMainMenu;
    }

    private void OnLoadMainMenu(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
            return;

        SceneManager.sceneLoaded -= OnLoadMainMenu;
        SendMainMenuLoadedEvent();
    }

    // Loading gameplay

    public void LoadGameplay(string room)
    {
        _nextRoom = room;
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLoadGameplay;
    }

    private void OnLoadGameplay(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Gameplay")
            return;

        SceneManager.sceneLoaded -= OnLoadGameplay;
        SendGameplayLoadedEvent();
        LoadAllScenes(_nextRoom);
    }

    // Changing rooms

    public void ChangeRoom(string room)
    {
        if (SceneManager.GetActiveScene().name != "Gameplay")
            throw new Exception("Tried to change room while not in gameplay");

        Debug.Log($"Changing room to {room}");
        _nextRoom = room;
        UnloadAllScenes(CurrentRoom, UnloadRoomsBeforeTransition);
    }

    private void UnloadRoomsBeforeTransition(string _)
    {
        LoadAllScenes(_nextRoom);
    }

    // Load / Unload

    private void LoadAllScenes(string room)
    {
        _loadRoom = room;
        _loadCallback = null;
        _loadCounter = SCENES_PER_ROOM;

        SceneManager.LoadSceneAsync(GetLayoutPath(room), LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(GetLogicPath(room), LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnLoadRoom;
    }

    private void OnLoadRoom(Scene _, LoadSceneMode __)
    {
        if (--_loadCounter == 0)
        {
            SceneManager.sceneLoaded -= OnLoadRoom;
            SendRoomLoadedEvent(_loadRoom);
            //_loadCallback(_loadRoom);
        }
    }

    private void UnloadAllScenes(string room, Action<string> callback)
    {
        _unloadRoom = room;
        _unloadCallback = callback;
        _unloadCounter = SCENES_PER_ROOM;

        SceneManager.UnloadSceneAsync(GetLayoutPath(room));
        SceneManager.UnloadSceneAsync(GetLogicPath(room));
        SceneManager.sceneUnloaded += OnUnloadRoom;
    }

    private void OnUnloadRoom(Scene _)
    {
        if (--_unloadCounter == 0)
        {
            SceneManager.sceneUnloaded -= OnUnloadRoom;
            SendRoomUnloadedEvent(_unloadRoom);
            _unloadCallback(_unloadRoom);
        }
    }

    // Events

    private void SendMainMenuLoadedEvent()
    {
        Debug.Log($"Loaded main menu");
        OnMainMenuLoaded?.Invoke();
    }

    private void SendGameplayLoadedEvent()
    {
        Debug.Log($"Loaded gameplay");
        OnGameplayLoaded?.Invoke();
    }

    private void SendRoomLoadedEvent(string room)
    {
        Debug.Log($"Loaded room {room}");
        CurrentRoom = room;
        OnRoomLoaded?.Invoke(room);
    }

    private void SendRoomUnloadedEvent(string room)
    {
        Debug.Log($"Unloaded room {room}");
        CurrentRoom = string.Empty;
        OnRoomUnloaded?.Invoke(room);
    }

    private const int SCENES_PER_ROOM = 2;
    private const string GAMEPLAY_SCENE_PATH = "Design/Scenes/Ship";
    private string GetLayoutPath(string room) => $"{GAMEPLAY_SCENE_PATH}/{room}/LAYOUT";
    private string GetLogicPath(string room) => $"{GAMEPLAY_SCENE_PATH}/{room}/LOGIC";

    public delegate void LoadDelegate();
    public event LoadDelegate OnGameplayLoaded;
    public event LoadDelegate OnMainMenuLoaded;

    public delegate void RoomDelegate(string room);
    public event RoomDelegate OnRoomLoaded;
    public event RoomDelegate OnRoomUnloaded;
}
