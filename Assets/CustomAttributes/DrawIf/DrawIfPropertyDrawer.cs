using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
///Implementation of the Property Attribute.
/// </summary>
[CustomPropertyDrawer(typeof(DrawIfAttribute))]
public class DrawIfPropertyDrawer : PropertyDrawer
{
    #region Fields
    // Reference to the attribute on the property.
    private DrawIfAttribute drawIf;

    // Field that is being compared.
    private SerializedProperty compareField_A;
    private SerializedProperty compareField_B;
    private FieldInfo field_A;
    private FieldInfo field_B;

    private bool done_Once = false;


    
    #endregion

    /// <summary>
    /// Specify how tall the GUI for this field is in pixels.
    /// </summary>
    /// <param name="property">The target property</param>
    /// <param name="label">The label of the field (name of the property)</param>
    /// <returns>The height of the property in pixel.</returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //If the condition to show the field is not met and we don't want to see the Property 
        if (ShowMe(property) == false && drawIf.disablingType == DisablingType.DontDraw)
            return 0f;//we draw a property with pixel in heigth

        // The height of the property should be defaulted to the default height.
        return base.GetPropertyHeight(property, label);
    }

    /// <summary>
    /// Errors default to showing the property.
    /// </summary>
    private bool ShowMe(SerializedProperty property)
    {
        if(done_Once == false)
        {
            Find_CompareFields(property);
            done_Once = true;
        }
       
        if(compareField_A == null || compareField_B == null) { Debug.Log("Value NULLS"); return true; }

        else if(((IComparable)field_A.GetValue(compareField_A.serializedObject.targetObject)).CompareTo(field_B.GetValue(compareField_B.serializedObject.targetObject)) == 0)
        {
            Debug.Log("Same value");
            return true;
        }



        else { Debug.Log("Different value"); return false; }
 
    }



    /// <summary>
    /// Is the method that allows us to change the Inspector.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // If the condition is met, simply draw the field.
        if (ShowMe(property))
        {
            EditorGUI.PropertyField(position, property, label);
        } 
        //...check if the disabling type is read only. If it is, draw it disabled
        else if (drawIf.disablingType == DisablingType.ReadOnly)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }


    public void Find_CompareFields(SerializedProperty property)
    {
        drawIf = attribute as DrawIfAttribute;

        string path = "";

        // Searching for the property specified inside the comparedPropertyName field (DrawIfAttribute)
        if (property.propertyPath.Contains(".") && property.depth == 0)
            path = System.IO.Path.ChangeExtension(property.propertyPath, drawIf.compared_PropertyA);//In other script
        else
            path = drawIf.compared_PropertyA;//Inside the same script

        //Once the path of the compared property is defined we are basically going to get it
        compareField_A = property.serializedObject.FindProperty(path);

        if (compareField_A == null)//If the property is not found (maybe misstyping)
        {
            Debug.LogError("Cannot find property with name: " + path);
        }

        
        if (property.propertyPath.Contains(".") && property.depth == 0)
            path = System.IO.Path.ChangeExtension(property.propertyPath, drawIf.compared_PropertyB);//In other script
        else
            path = drawIf.compared_PropertyB;//Inside the same script

        compareField_B = property.serializedObject.FindProperty(path);

        if (compareField_B == null) {    Debug.LogError("Cannot find property with name: " + path);   }



        FieldInfo[] fields = compareField_A.serializedObject.targetObject.GetType().GetFields();

        foreach (FieldInfo field in fields)
        {
            if (field.Name == compareField_A.name)
            {
                Debug.Log(field.Name + "  .  " + field.GetValue(compareField_A.serializedObject.targetObject));
                field_A = field;
            }
            else if(field.Name == compareField_B.name)
            {
                Debug.Log(field.Name + "  .  " + field.GetValue(compareField_B.serializedObject.targetObject));
                field_B = field;
            }
        }

    }
}
