using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : BaseManager
{
    public override void OnInitialize()
    {
        GameObject prefab = Resources.Load<GameObject>("Canvas");
        GameObject canvas = Object.Instantiate(prefab);
        Object.DontDestroyOnLoad(canvas);
        Debug.Log("Created canvas object");
    }
}
