using System;
using System.Runtime.CompilerServices;

namespace MapEditor.Services.Properties.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CustomPropertyAttribute : Attribute, ICloneable
{
    public string PropertyName { get; }
    public string VisualizeName { get; set; }
    public bool IsReadOnly { get; set; }
    public string? ShowIfProperty { get; }
    public object? ShowIfValue { get; }
    
    public CustomPropertyAttribute(string visualizeName = "", bool isReadOnly = false, 
        string? showIfProperty = null, object? showIfValue = null, [CallerMemberName] string? calledMemberName = null)
    {
        PropertyName = calledMemberName ?? "";
        VisualizeName = string.IsNullOrEmpty(visualizeName) ? PropertyName : visualizeName;
        IsReadOnly = isReadOnly;
        ShowIfProperty = showIfProperty;
        ShowIfValue = showIfValue;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}