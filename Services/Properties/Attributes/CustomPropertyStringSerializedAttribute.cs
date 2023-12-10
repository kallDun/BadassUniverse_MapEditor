using System;
using Newtonsoft.Json;

namespace MapEditor.Services.Properties.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CustomPropertyStringSerializedAttribute : CustomPropertyAttribute
{
    private readonly Type[] types;
    
    public CustomPropertyStringSerializedAttribute(string propertyName = "", bool isReadOnly = false, params Type[] types) 
        : base(propertyName, isReadOnly)
    {
        this.types = types;
    }
    
    public object? Deserialize(string? value)
    {
        if (value == null) return null;
        foreach (Type type in types)
        {
            object? obj = JsonConvert.DeserializeObject(value, type);
            if (obj != null) return obj;
        }
        return null;
    }
}