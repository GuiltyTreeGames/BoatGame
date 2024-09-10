using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Interactable
{
    protected override void OnInteract()
    {
        Debug.Log("Shooting cannon");
    }
}
