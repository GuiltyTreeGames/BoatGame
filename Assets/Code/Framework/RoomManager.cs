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
        SceneManager.sceneLoaded += OnSceneLoaded;
        string room = SceneManager.GetActiveScene().name;
        if (room != "MainMenu")
        {
            CurrentRoom = room;
            OnRoomLoaded?.Invoke(room);
        }
    }

    public void ChangeRoom(string room)
    {
        Debug.Log("Changing to room " + room);

        // Very temporary to do this
        tempObject.StartCoroutine(FadeOutCoroutine(room));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
            return;

        SceneManager.SetActiveScene(scene);
        SceneManager.UnloadSceneAsync(PreviousRoom);

        // Very temporary to do this
        tempObject.StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeOutCoroutine(string room)
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

        PreviousRoom = CurrentRoom;
        CurrentRoom = string.Empty;
        NextRoom = room;

        Debug.Log($"Unloaded room {PreviousRoom}");
        OnRoomUnloaded?.Invoke(PreviousRoom);
        SceneManager.LoadScene(room, LoadSceneMode.Additive);
    }

    IEnumerator FadeInCoroutine()
    {
        CurrentRoom = NextRoom;
        NextRoom = string.Empty;

        Debug.Log($"Loaded room {CurrentRoom}");
        OnRoomLoaded?.Invoke(CurrentRoom);

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
