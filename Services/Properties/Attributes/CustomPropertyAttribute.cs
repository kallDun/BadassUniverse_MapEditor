using System;

namespace MapEditor.Services.Properties.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CustomPropertyAttribute : Attribute
{
    public string PropertyName { get; }
    public bool IsReadOnly { get; }
    public string? HideIfProperty { get; }
    public object? HideIfValue { get; }
    
    public CustomPropertyAttribute(string propertyName = "", bool isReadOnly = false, 
        string? hideIfProperty = null, object? hideIfValue = null)
    {
        PropertyName = propertyName;
        IsReadOnly = isReadOnly;
        HideIfProperty = hideIfProperty;
        HideIfValue = hideIfValue;
    }
}