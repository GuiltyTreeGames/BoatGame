using UnityEngine;
using UnityEngine.UI;

public class FadeWidget : MonoBehaviour
{
    private Image image;

    private void OnEnable()
    {
        Core.RoomManager.OnFade += OnFade;
    }

    private void OnDisable()
    {
        Core.RoomManager.OnFade -= OnFade;
    }

    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color32(0, 0, 0, 0);
    }

    void OnFade(float percent)
    {
        image.color = new Color32(0, 0, 0, (byte)(percent * 255));
    }
}
