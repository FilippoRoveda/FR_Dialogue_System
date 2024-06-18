using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

/// <summary>
/// Base class for DialogueSystem node group.
/// </summary>
public class DS_Group : Group
{
    public string ID { get; set; }

    public string oldTitle;

    private Color defaultBorderColor;
    private float defaultBorderWidth;

    public DS_Group(string title, Vector2 spawnPosition, string ID = null)
    {
        if (ID == null) this.ID = Guid.NewGuid().ToString();
        else this.ID = ID;
        this.title = title;
        oldTitle = title;

        SetPosition(new Rect(spawnPosition, Vector2.zero));

        defaultBorderColor = contentContainer.style.borderBottomColor.value;
        defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
    }
    /// <summary>
    /// Update the group style according to the presence of errors related to this group.
    /// </summary>
    /// <param name="errorColor"></param>
    public void SetErrorStyle(Color errorColor)
    {
        contentContainer.style.borderBottomColor = errorColor;
        contentContainer.style.borderBottomWidth = 2.0f;
    }

    /// <summary>
    /// Reset the style of that group to default state.
    /// </summary>
    public void ResetStyle()
    {
        contentContainer.style.borderBottomColor = defaultBorderColor;
        contentContainer.style.borderBottomWidth= defaultBorderWidth;
    }
}
