using System;

namespace MapEditor.Services.Properties.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CustomPropertyAttribute : Attribute
{
    public string PropertyName { get; set; }
    public bool IsReadOnly { get; set; }
    
    public CustomPropertyAttribute(string propertyName = "", bool isReadOnly = false)
    {
        PropertyName = propertyName;
        IsReadOnly = isReadOnly;
    }
}