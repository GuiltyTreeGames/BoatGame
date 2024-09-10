using System.Collections.Generic;

public class InputBlock
{
    public IEnumerable<InputType> BlockedInputs { get; }

    public InputBlock(IEnumerable<InputType> blockedInputs)
    {
        BlockedInputs = blockedInputs;
    }
}
