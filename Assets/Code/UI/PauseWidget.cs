using UnityEngine;

public class PauseWidget : MonoBehaviour
{
    [SerializeField]
    private GameObject _content;

    void Awake()
    {
        HidePauseMenu();
    }

    void Update()
    {
        if (!Core.InputManager.GetButtonDown(InputType.Pause))
            return;

        if (_content.activeSelf)
            HidePauseMenu();
        else
            ShowPauseMenu();
    }

    public void ShowPauseMenu()
    {
        Debug.Log("Pausing game");
        Core.InputManager.AddInputBlock(PAUSE_BLOCK);
        Time.timeScale = 0;

        _content.SetActive(true);
    }

    public void HidePauseMenu()
    {
        Debug.Log("Unpausing game");
        Core.InputManager.RemoveInputBlock(PAUSE_BLOCK);
        Time.timeScale = 1;

        _content.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Debug.Log("Quitting to menu");
    }

    private static readonly InputBlock PAUSE_BLOCK = new InputBlock(new InputType[]
    {
        InputType.MoveHorizontal,
        InputType.MoveVertical,
        InputType.Interact
    });
}
