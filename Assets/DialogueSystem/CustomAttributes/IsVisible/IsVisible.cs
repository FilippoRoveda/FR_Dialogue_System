using UnityEngine;

/// <summary>
/// Custom attriute to make field visible in inspector based on a boolean value.
/// </summary>
public class IsVisible : PropertyAttribute
{
    public bool isVisible { get; private set; }

    public IsVisible(bool isVisible)
    {
        this.isVisible = isVisible;
    }
}
