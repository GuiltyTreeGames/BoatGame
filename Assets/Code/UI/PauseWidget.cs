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
        Core.InputManager.AddInputBlock(PAUSE_BLOCK);
        _content.SetActive(true);
    }

    public void HidePauseMenu()
    {
        _content.SetActive(false);
        Core.InputManager.RemoveInputBlock(PAUSE_BLOCK);
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
