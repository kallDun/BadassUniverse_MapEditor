using System;

namespace BadassUniverse_MapEditor.Extensions.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class PropertyAttribute : Attribute
{
    public string PropertyName { get; set; }
    public bool IsReadOnly { get; set; }
    
    public PropertyAttribute(string propertyName = "", bool isReadOnly = false)
    {
        PropertyName = propertyName;
        IsReadOnly = isReadOnly;
    }
}