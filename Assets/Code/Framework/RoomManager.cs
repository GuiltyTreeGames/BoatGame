using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : BaseManager
{
    public override void OnInitialize()
    {
    }

    public void ChangeRoom(string room)
    {
        Debug.Log("Changing to room " + room);
        SceneManager.LoadSceneAsync(room);
    }
}
