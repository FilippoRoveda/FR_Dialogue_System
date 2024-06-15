using UnityEngine;

/// <summary>
/// Custom attriute to make field interactabòe in inspector based on a boolean value.
/// </summary>
public class IsInteractable : PropertyAttribute
{
    public bool isInteractable { get; private set; }

    public IsInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }
}
