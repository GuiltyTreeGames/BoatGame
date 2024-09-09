using UnityEngine;

public class OpenMainMenu : MonoBehaviour
{
    void Start()
    {
        Core.CanvasManager.MainMenuWidget.SetMenuStatus(true);
    }
}
