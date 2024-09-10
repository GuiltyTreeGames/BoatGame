using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MovementDirection { get; private set; }

    void Update()
    {
        float x = Core.InputManager.GetAxis(InputType.MoveHorizontal);
        float y = Core.InputManager.GetAxis(InputType.MoveVertical);
        MovementDirection = new Vector2(x, y);
    }
}
