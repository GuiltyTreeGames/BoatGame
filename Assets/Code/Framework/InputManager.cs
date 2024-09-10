using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : BaseManager
{
    private readonly List<InputBlock> _inputBlocks = new();

    public void AddInputBlock(InputBlock block)
    {
        if (!_inputBlocks.Contains(block))
            _inputBlocks.Add(block);
        Debug.Log($"Adding input block: {_inputBlocks.Count}");
    }

    public void RemoveInputBlock(InputBlock block)
    {
        _inputBlocks.Remove(block);
        Debug.Log($"Removing input block: {_inputBlocks.Count}");
    }

    public bool GetButton(InputType input)
    {
        return !IsInputBlocked(input) && Input.GetButton(input.ToString());
    }

    public bool GetButtonDown(InputType input)
    {
        return !IsInputBlocked(input) && Input.GetButtonDown(input.ToString());
    }

    public bool GetButtonUp(InputType input)
    {
        return !IsInputBlocked(input) && Input.GetButtonUp(input.ToString());
    }

    public float GetAxis(InputType input)
    {
        return !IsInputBlocked(input) ? Input.GetAxisRaw(input.ToString()) : 0f;
    }

    public bool IsInputBlocked(InputType input)
    {
        return _inputBlocks.Where(x => x.BlockedInputs.Contains(InputType.Any) || x.BlockedInputs.Contains(input)).Count() > 0;
    }
}
