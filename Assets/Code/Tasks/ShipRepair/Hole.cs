using UnityEngine;
using UnityEngine.EventSystems;

public class Hole : MonoBehaviour, IDropHandler
{
    private bool _isPatched = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (_isPatched)
            return;

        Plank plank = eventData.pointerDrag.GetComponent<Plank>();

        if (plank == null)
        {
            Debug.LogError("A non-plank object was dragged onto a hole!");
            return;
        }

        _isPatched = true;
        plank.Patch();
        plank.transform.position = transform.position;
    }
}
