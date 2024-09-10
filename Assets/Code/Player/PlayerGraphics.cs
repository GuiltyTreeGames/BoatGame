using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Core.InputManager.GetAxis(InputType.MoveHorizontal);
        float v = Core.InputManager.GetAxis(InputType.MoveVertical);

        if (h != 0 || v != 0)
        {
            anim.SetFloat("x", h);
            anim.SetFloat("y", v);
        }
    }
}
