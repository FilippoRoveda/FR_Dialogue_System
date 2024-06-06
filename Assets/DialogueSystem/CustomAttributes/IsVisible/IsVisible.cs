using UnityEngine;

public class IsVisible : PropertyAttribute
{
    public bool isVisible { get; private set; }

    public IsVisible(bool isVisible)
    {
        this.isVisible = isVisible;
    }
}
