using UnityEngine;

/// <summary>
/// Custom attriute to make field interactab�e in inspector based on a boolean value.
/// </summary>
public class IsInteractable : PropertyAttribute
{
    public bool isInteractable { get; private set; }
    public string ConditionFieldName { get; private set; }

    public IsInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
        ConditionFieldName = string.Empty;
    }
    public IsInteractable(string conditionFieldName)
    {
        isInteractable = false;
        ConditionFieldName = conditionFieldName;
    }
}
