using UnityEngine;
using UnityEngine.SceneManagement;

public class NewRoomManager : BaseManager
{
    public string CurrentRoom { get; private set; } = string.Empty;

    public void LoadMainMenu()
    {
        Debug.Log($"Loading main menu");
        CurrentRoom = string.Empty;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void OnLoadMainMenu(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLoadMainMenu;

        OnMainMenuLoaded?.Invoke();
    }

    public void LoadGameplay(string room)
    {
        Debug.Log($"Loading gameplay ({room})");
        CurrentRoom = room;

        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLoadGameplay;
        LoadAllScenes(room);
    }

    private void OnLoadGameplay(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLoadGameplay;

        OnGameplayLoaded?.Invoke();
    }

    public void ChangeRoom(string room)
    {
        if (SceneManager.GetActiveScene().name != "Gameplay")
            throw new System.Exception("Tried to change room while not in gameplay");

        Debug.Log($"Changing room to {room}");
        UnloadAllScenes(CurrentRoom);
        CurrentRoom = room;
        LoadAllScenes(CurrentRoom);
    }

    private void LoadAllScenes(string room)
    {
        SceneManager.LoadSceneAsync(GetLayoutPath(room), LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(GetLogicPath(room), LoadSceneMode.Additive);
    }

    private void UnloadAllScenes(string room)
    {
        SceneManager.UnloadSceneAsync(GetLayoutPath(CurrentRoom));
        SceneManager.UnloadSceneAsync(GetLogicPath(CurrentRoom));
    }

    private string GetLayoutPath(string room) => $"Design/Scenes/Ship/{room}/LAYOUT";
    private string GetLogicPath(string room) => $"Design/Scenes/Ship/{room}/LOGIC";

    public delegate void LoadDelegate();
    public event LoadDelegate OnGameplayLoaded;
    public event LoadDelegate OnMainMenuLoaded;

    public delegate void RoomDelegate(string room);
    public event RoomDelegate OnRoomLoaded;
    public event RoomDelegate OnRoomUnloaded;
}
