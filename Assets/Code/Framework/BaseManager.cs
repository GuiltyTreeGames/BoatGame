using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager
{
    public virtual void OnInitialize() { }

    public virtual void OnAllInitialized() { }

    public virtual void OnDispose() { }

    public virtual void OnEnterGame() { }

    public virtual void OnExitGame() { }

    //public virtual void OnRoomLoaded(string room) { }

    //public virtual void OnRoomUnloaded(string room) { }
}
