using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MapEditor.Services.Properties.Data;

public class PropertyDataEvents
{
    private readonly Action<object?> setValue;
    private readonly Func<object?> getValue;
    
    public PropertyDataEvents(Action<object?> setValue, Func<object?> getValue)
    {
        this.setValue = setValue;
        this.getValue = getValue;
    }
    
    public void SetValue(object? value)
    {
        setValue(value);
    }
    
    public object? GetValue()
    {
        return getValue();
    }
    
    public static PropertyDataEvents FromPropertyInfo(object item, PropertyInfo propertyInfo)
    {
        return new PropertyDataEvents(
            value => propertyInfo.SetValue(item, value),
            () => propertyInfo.GetValue(item)
        );
    }

    public static PropertyDataEvents FromList(IEnumerable<object> list, int index)
    {
        return new PropertyDataEvents(Setter, Getter);

        object? Getter() => list.ElementAt(index);

        void Setter(object? value)
        {
            if (value == null) return;
            list = list.Select((x, i) => i == index ? value : x);
        }
    }
}