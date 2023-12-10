using System.Runtime.CompilerServices;

namespace MapEditor.Services.Properties.Attributes;

public class CustomPropertyListAttribute : CustomPropertyAttribute
{
    public bool IsItemReadOnly { get; }
    
    public CustomPropertyListAttribute(string visualizeName = "", bool isReadOnly = false, bool isItemReadOnly = false, [CallerMemberName] string? calledMemberName = null) 
        : base(visualizeName, isReadOnly, calledMemberName: calledMemberName)
    {
        IsItemReadOnly = isItemReadOnly;
    }
}