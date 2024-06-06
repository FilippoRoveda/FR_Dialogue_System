using UnityEngine;

public class IsInteractable : PropertyAttribute
{
    public bool isInteractable { get; private set; }

    public IsInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }
}
