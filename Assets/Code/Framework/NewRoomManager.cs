using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewRoomManager : BaseManager
{
    public string CurrentRoom { get; private set; } = string.Empty;

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
        CurrentRoom = string.Empty;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
        CurrentRoom = room;

        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLoadGameplay;
        LoadAllScenes(room);
    }

    private void OnLoadGameplay(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Gameplay")
            return;

        SceneManager.sceneLoaded -= OnLoadGameplay;
        SendGameplayLoadedEvent();
    }

    // Changing rooms

    public void ChangeRoom(string room)
    {
        if (SceneManager.GetActiveScene().name != "Gameplay")
            throw new System.Exception("Tried to change room while not in gameplay");

        Debug.Log($"Changing room to {room}");
        UnloadAllScenes(CurrentRoom);
        LoadAllScenes(room);
        CurrentRoom = room;
    }

    private void LoadAllScenes(string room)
    {
        SceneManager.LoadSceneAsync(GetLayoutPath(room), LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(GetLogicPath(room), LoadSceneMode.Additive);
    }

    private void UnloadAllScenes(string room)
    {
        SceneManager.UnloadSceneAsync(GetLayoutPath(room));
        SceneManager.UnloadSceneAsync(GetLogicPath(room));
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
        OnRoomLoaded?.Invoke(room);
    }

    private void SendRoomUnloadedEvent(string room)
    {
        Debug.Log($"Unloaded room {room}");
        OnRoomUnloaded?.Invoke(room);
    }

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
