using UnityEngine;

public class SelectableGroup : MonoBehaviour
{
    [SerializeField] SelectableOption[] _selectables;
    [SerializeField] bool _vertical;

    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetSelection(value);
    }

    private SelectableOption SelectedElement => _selectables[_selectedIndex];

    private void OnEnable()
    {
        foreach (var selectable in _selectables)
            selectable.OnDeselect();

        _selectables[_selectedIndex = 0].OnSelect();
    }

    private void Update()
    {
        float change = Core.InputManager.GetAxisDown(_vertical ? InputType.UIVertical : InputType.UIHorizontal);
        if (change != 0)
        {
            ChangeSelection((int)change);
        }

        if (Core.InputManager.GetButtonDown(InputType.UIConfirm))
            SelectedElement.OnClick();
    }

    private void ChangeSelection(int diff)
    {
        SelectedElement.OnDeselect();

        _selectedIndex += diff;

        if (_selectedIndex < 0)
            _selectedIndex = _selectables.Length - 1;
        else if (_selectedIndex >= _selectables.Length)
            _selectedIndex = 0;

        SelectedElement.OnSelect();
    }

    private void SetSelection(int index)
    {
        SelectedElement.OnDeselect();
        _selectedIndex = index;
        SelectedElement.OnSelect();
    }
}
