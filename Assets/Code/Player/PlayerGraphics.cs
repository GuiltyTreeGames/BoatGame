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

        bool isMoving = h != 0 || v != 0;
        if (isMoving)
        {
            anim.SetFloat("x", h);
            anim.SetFloat("y", v);
        }
        anim.SetBool("isMoving", isMoving);
    }
}
