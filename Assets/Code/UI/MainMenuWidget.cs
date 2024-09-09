using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWidget : MonoBehaviour
{
    [SerializeField]
    private string _firstScene;

    public void StartGame()
    {
        Debug.Log("Starting game");
        SceneManager.LoadScene(_firstScene);
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
}
