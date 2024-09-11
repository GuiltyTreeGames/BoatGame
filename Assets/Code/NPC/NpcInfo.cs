using UnityEngine;

public class NpcInfo : MonoBehaviour
{
    public bool isDead;
    public SpawnInfo spawnInfo = new();

    private void Start()
    {
        isDead = false;
        spawnInfo.preservePositionX = true;
        spawnInfo.preservePositionY = true;
    }
}
