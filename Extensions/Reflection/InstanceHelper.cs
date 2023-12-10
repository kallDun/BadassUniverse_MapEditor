using System;
using System.Collections.Generic;

namespace MapEditor.Extensions.Reflection;

public static class InstanceHelper
{
    public static object? GetNewInstance(Type type)
    {
        // if string, return empty string
        if (type == typeof(string)) return "";
        // if enum, return first value
        if (type.IsEnum) return Enum.GetValues(type).GetValue(0);
        // if nullable, return null
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) return null;
        // if list, return empty list
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var listType = typeof(List<>);
            var genericArgs = type.GetGenericArguments();
            var concreteType = listType.MakeGenericType(genericArgs);
            return Activator.CreateInstance(concreteType);
        }
        // if dictionary, return empty dictionary
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            var listType = typeof(Dictionary<,>);
            var genericArgs = type.GetGenericArguments();
            var concreteType = listType.MakeGenericType(genericArgs);
            return Activator.CreateInstance(concreteType);
        }
        // if array, return empty array
        if (type.IsArray)
        {
            var listType = typeof(List<>);
            var genericArgs = type.GetGenericArguments();
            var concreteType = listType.MakeGenericType(genericArgs);
            return Activator.CreateInstance(concreteType);
        }
        // if class, return new instance
        return Activator.CreateInstance(type);
    }
}