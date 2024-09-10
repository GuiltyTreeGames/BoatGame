using UnityEngine;

public class MainMenuWidget : MonoBehaviour
{
    [SerializeField]
    private string _firstScene;
    [SerializeField]
    private GameObject _content;

    void Awake()
    {
        SetMenuStatus(false);
    }

    public void StartGame()
    {
        Debug.Log("Starting game");
        Core.RoomManager.ChangeRoomInstant(_firstScene);
        SetMenuStatus(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetMenuStatus(bool open)
    {
        _content.SetActive(open);
    }
}
