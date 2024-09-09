using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : BaseManager
{
    private ChangeRoomOnTrigger tempObject;

    public override void OnInitialize()
    {
        tempObject = new GameObject("Temp").AddComponent<ChangeRoomOnTrigger>();
        Object.DontDestroyOnLoad(tempObject.gameObject);
    }

    public void ChangeRoom(string room)
    {
        Debug.Log("Changing to room " + room);

        // Very temporary to do this
        tempObject.StartCoroutine(FadeCoroutine(room));
    }

    IEnumerator FadeCoroutine(string room)
    {
        float startTime = Time.time;
        Debug.Log("Starting fade out");

        while (true)
        {
            float percent = Mathf.Clamp01((Time.time - startTime) / FADE_TIME);
            OnFade?.Invoke(percent);

            yield return new WaitForEndOfFrame();

            if (percent >= 1)
                break;
        }

        SceneManager.LoadSceneAsync(room);

        startTime = Time.time;
        Debug.Log("Starting fade in");

        while (true)
        {
            float percent = Mathf.Clamp01((Time.time - startTime) / FADE_TIME);
            OnFade?.Invoke(1 - percent);

            yield return new WaitForEndOfFrame();

            if (percent >= 1)
                break;
        }
    }

    public delegate void FadeDelegate(float percent);
    public event FadeDelegate OnFade;

    private const float FADE_TIME = 0.5f;
}
