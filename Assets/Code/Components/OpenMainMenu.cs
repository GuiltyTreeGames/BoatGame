using UnityEngine;

public class OpenMainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Core.InputManager.AddInputBlock(MENU_BLOCK);
    }

    private void OnDisable()
    {
        Core.InputManager.RemoveInputBlock(MENU_BLOCK);
    }

    void Start()
    {
        Core.CanvasManager.MainMenuWidget.SetMenuStatus(true);
    }

    private static readonly InputBlock MENU_BLOCK = new InputBlock(new InputType[]
    {
        InputType.MoveHorizontal,
        InputType.MoveVertical,
        InputType.Interact,
        InputType.Pause
    });
}
