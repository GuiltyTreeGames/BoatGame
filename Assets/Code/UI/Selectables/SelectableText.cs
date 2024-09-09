using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectableText : SelectableOption
{
    private TMP_Text x_text;
    private TMP_Text TextElement
    {
        get
        {
            if (x_text == null)
                x_text = GetComponent<TMP_Text>();
            return x_text;
        }
    }

    [SerializeField]
    private Color _normalColor = Color.white;
    [SerializeField]
    private Color _selectedColor = Color.yellow;
    [SerializeField]
    private UnityEvent onClick;

    public override void OnSelect()
    {
        TextElement.color = _selectedColor;
    }

    public override void OnDeselect()
    {
        TextElement.color = _normalColor;
    }

    public override void OnClick()
    {
        onClick?.Invoke();
    }
}
