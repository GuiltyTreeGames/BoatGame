using UnityEngine;

public class CanvasManager : BaseManager
{
    public override void OnInitialize()
    {
        GameObject prefab = Resources.Load<GameObject>("Canvas");
        GameObject canvas = Object.Instantiate(prefab);
        Object.DontDestroyOnLoad(canvas);

        FadeWidget = canvas.GetComponentInChildren<FadeWidget>();
        MainMenuWidget = canvas.GetComponentInChildren<MainMenuWidget>();

        Debug.Log("Created canvas object");
    }

    public FadeWidget FadeWidget { get; private set; }
    public MainMenuWidget MainMenuWidget { get; private set; }
}
