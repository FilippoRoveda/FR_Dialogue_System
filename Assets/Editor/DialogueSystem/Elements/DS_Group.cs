using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DS_Group : Group
{
    public string ID { get; private set; }
    public string oldTitle;

    private Color defaultBorderColor;
    private float defaultBorderWidth;

    public DS_Group(string title, Vector2 spawnPosition) //Here check if value ID is null for a new instantiation
    {
        ID = Guid.NewGuid().ToString();
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
