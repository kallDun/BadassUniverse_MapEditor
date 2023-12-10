using System;

namespace MapEditor.Services.Properties.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CustomPropertyAttribute : Attribute
{
    public string PropertyName { get; }
    public bool IsReadOnly { get; }
    public string? ShowIfProperty { get; }
    public object? ShowIfValue { get; }
    
    public CustomPropertyAttribute(string propertyName = "", bool isReadOnly = false, 
        string? showIfProperty = null, object? showIfValue = null)
    {
        PropertyName = propertyName;
        IsReadOnly = isReadOnly;
        ShowIfProperty = showIfProperty;
        ShowIfValue = showIfValue;
    }
}