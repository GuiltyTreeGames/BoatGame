
[System.Flags]
public enum InputType
{
    MoveHorizontal = 0x0001,
    MoveVertical = 0x0002,
    UIHorizontal = 0x0004,
    UIVertical = 0x0008,

    Pause = 0x0010,
    Interact = 0x0020,
    UIConfirm = 0x0040,
    UITabLeft = 0x0080,

    UITabRight = 0x0100,

    Any = 0xFFFF
}
