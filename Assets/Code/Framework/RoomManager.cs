using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : BaseManager
{
    private DoorEnterOnTrigger tempObject;

    public string PreviousRoom { get; private set; } = string.Empty;
    public string CurrentRoom { get; private set; } = string.Empty;
    public string NextRoom { get; private set; } = string.Empty;

    public override void OnInitialize()
    {
        tempObject = new GameObject("Temp").AddComponent<DoorEnterOnTrigger>();
        Object.DontDestroyOnLoad(tempObject.gameObject);
    }

    public override void OnAllInitialized()
    {
        // If not starting in main menu, fake a scene load event
        string room = SceneManager.GetActiveScene().name;
        if (room != "MainMenu")
        {
            CurrentRoom = room;
            OnRoomLoaded?.Invoke(room);
        }
    }

    /// <summary>
    /// Updates the room properties and calls the unload event
    /// </summary>
    private void PerformUnload(string room)
    {
        PreviousRoom = CurrentRoom;
        CurrentRoom = string.Empty;
        NextRoom = room;

        Debug.Log($"Unloaded room {PreviousRoom}");
        OnRoomUnloaded?.Invoke(PreviousRoom);
    }

    /// <summary>
    /// Updates the room properties and calls the load event
    /// </summary>
    private void PerformLoad()
    {
        CurrentRoom = NextRoom;
        NextRoom = string.Empty;

        Debug.Log($"Loaded room {CurrentRoom}");
        OnRoomLoaded?.Invoke(CurrentRoom);
    }

    /// <summary>
    /// Step 0: Load the next room
    /// </summary>
    public void ChangeRoomInstant(string room)
    {
        PerformUnload(room);

        SceneManager.sceneLoaded += OnOpenNextInstant;
        SceneManager.LoadScene(room);
    }

    /// <summary>
    /// Step 1: Finish up
    /// </summary>
    private void OnOpenNextInstant(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnOpenNextInstant;

        PerformLoad();
    }

    /// <summary>
    /// Step 0: Start process
    /// </summary>
    public void ChangeRoomWithFade(string room)
    {
        Debug.Log("Changing to room " + room);

        // Very temporary to do this
        tempObject.StartCoroutine(FadeOutCoroutine(room));
    }

    /// <summary>
    /// Step 1: Fade out, then load the loading level
    /// </summary>
    private IEnumerator FadeOutCoroutine(string room)
    {
        float startTime = Time.time;
        Debug.Log("Starting fade out");

        while (true)
        {
            float percent = Mathf.Clamp01((Time.time - startTime) / FADE_TIME);
            OnFade?.Invoke(percent);

            yield return null;

            if (percent >= 1)
                break;
        }

        PerformUnload(room);

        SceneManager.sceneLoaded += OnOpenLoading;
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Step 2: Unload the previous room
    /// </summary>
    private void OnOpenLoading(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnOpenLoading;

        SceneManager.SetActiveScene(scene);
        SceneManager.UnloadSceneAsync(PreviousRoom);

        SceneManager.sceneUnloaded += OnClosePrevious;
    }

    /// <summary>
    /// Step 3: Load the next room
    /// </summary>
    private void OnClosePrevious(Scene scene)
    {
        SceneManager.sceneUnloaded -= OnClosePrevious;

        SceneManager.LoadScene(NextRoom, LoadSceneMode.Additive);

        SceneManager.sceneLoaded += OnOpenNext;
    }

    /// <summary>
    /// Step 4: Unload the loading level
    /// </summary>
    private void OnOpenNext(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnOpenNext;

        SceneManager.SetActiveScene(scene);
        SceneManager.UnloadSceneAsync("Loading");

        // Very temporary to do this
        tempObject.StartCoroutine(FadeInCoroutine());
    }

    /// <summary>
    /// Step 5: Fade in
    /// </summary>
    private IEnumerator FadeInCoroutine()
    {
        PerformLoad();

        float startTime = Time.time;
        Debug.Log("Starting fade in");

        while (true)
        {
            float percent = Mathf.Clamp01((Time.time - startTime) / FADE_TIME);
            OnFade?.Invoke(1 - percent);

            yield return null;

            if (percent >= 1)
                break;
        }
    }

    public delegate void RoomDelegate(string room);
    public event RoomDelegate OnRoomLoaded;
    public event RoomDelegate OnRoomUnloaded;

    public delegate void FadeDelegate(float percent);
    public event FadeDelegate OnFade;

    private const float FADE_TIME = 0.5f;
}
