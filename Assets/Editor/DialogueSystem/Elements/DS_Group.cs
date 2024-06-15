using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Base class for DialogueSystem node group.
/// </summary>
public class DS_Group : Group
{
    public string ID { get; set; }

    public string oldTitle;

    private Color defaultBorderColor;
    private float defaultBorderWidth;

    public DS_Group(string title, Vector2 spawnPosition, string ID = null) /
    {
        if (ID == null) this.ID = Guid.NewGuid().ToString();
        else this.ID = ID;
        this.title = title;
        oldTitle = title;

        SetPosition(new Rect(spawnPosition, Vector2.zero));

        defaultBorderColor = contentContainer.style.borderBottomColor.value;
        defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
    }

    public void SetErrorStyle(Color errorColor)
    {
        contentContainer.style.borderBottomColor = errorColor;
        contentContainer.style.borderBottomWidth = 2.0f;
    }
    public void ResetStyle()
    {
        contentContainer.style.borderBottomColor = defaultBorderColor;
        contentContainer.style.borderBottomWidth= defaultBorderWidth;
    }
}
