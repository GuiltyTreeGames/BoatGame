using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewRoomManager : BaseManager
{
    public void LoadMainMenu()
    {
        Debug.Log($"Loading main menu");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void LoadGameplay(string room)
    {
        Debug.Log($"Loading gameplay ({room})");
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync($"Design/Scenes/Ship/{room}/LAYOUT", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync($"Design/Scenes/Ship/{room}/LOGIC", LoadSceneMode.Additive);
    }
}
