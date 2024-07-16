using UnityEngine;
using System;

/// <summary>
/// Types of comparisons.
/// </summary>
public enum DisablingType
{
    ReadOnly,//The field is show disabled
    DontDraw//The field is not show
}

/// <summary>
/// Draws the field/property ONLY if the value of the comparedPropertyName is matching with the comparedValue.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DrawIfAttribute : PropertyAttribute
{
    #region Fields
    public string compared_PropertyA { get; private set; }
    public string compared_PropertyB { get; private set; }

    public DisablingType disablingType { get; private set; }


    #endregion

    /// <summary>
    /// Constructor. This method is the one that will gonna describe our attribute and decide which parameters we need to use.
    /// Only draws the field if a condition is met. Supports enum and bools (but we can add other types of conditions such as comparison).
    /// </summary>
    /// <param name="comparedPropertyName">The name of the property that is being compared (case sensitive).</param>
    /// <param name="compared_PropertyB">The name of the second property to being compared to.</param>
    /// <param name="disablingType">The type of disabling that should happen if the condition is NOT met. Defaulted to DisablingType.DontDraw.</param>
    public DrawIfAttribute(string comparedPropertyNameA, string comparedPropertyNameB, DisablingType disablingType)
    {
        this.compared_PropertyA = comparedPropertyNameA;
        this.compared_PropertyB = comparedPropertyNameB;
        this.disablingType = disablingType;
    }
}