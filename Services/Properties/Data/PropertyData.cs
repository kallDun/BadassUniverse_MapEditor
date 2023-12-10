using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapEditor.Models.Server;
using MapEditor.Services.Properties.Attributes;

namespace MapEditor.Services.Properties.Data;

public class PropertyData
{
    private readonly object Item;
    private readonly PropertyInfo PropertyInfo;
    private readonly CustomPropertyAttribute Attribute;
    
    public string VisualizedName { get; private set; }
    public bool IsReadOnly { get; private set; }
    public bool IsVisible { get; private set; }
    public object? Value { get; private set; }
    public List<PropertyData>? SubProperties { get; private set; }
    public Action? OnValueChangedFromWorld { get; set; }
    public Action? OnValueChangedFromProperties { get; set; }
    public Action? OnVisibilityChanged { get; set; }
    
    public PropertyData(object item, PropertyInfo propertyInfo, CustomPropertyAttribute attribute)
    {
        Item = item;
        PropertyInfo = propertyInfo;
        Attribute = attribute;

        VisualizedName = string.IsNullOrEmpty(attribute.PropertyName) ? propertyInfo.Name : attribute.PropertyName;
        IsReadOnly = attribute.IsReadOnly;
        IsVisible = true;
        Value = propertyInfo.GetValue(item);
        OnValueChangedFromWorld += () => Value = propertyInfo.GetValue(item);

        if (item is AItemDTO itemDto)
        {
            itemDto.OnValueChanged += (propertyName) =>
            {
                if (propertyName != propertyInfo.Name) return;
                OnValueChangedFromWorld?.Invoke();
            };
        }
        
        CreateSubProperties();
    }

    public void InitRelations(IEnumerable<PropertyData> properties)
    {
        if (Attribute.ShowIfProperty == null) return;
        var relatedProperty = properties.FirstOrDefault(x => x.PropertyInfo.Name == Attribute.ShowIfProperty);
        if (relatedProperty == null) return;
        ChangePropertyVisibility(relatedProperty);
        relatedProperty.OnValueChangedFromProperties += () => ChangePropertyVisibility(relatedProperty);
        relatedProperty.OnValueChangedFromWorld += () => ChangePropertyVisibility(relatedProperty);
    }
    
    private void ChangePropertyVisibility(PropertyData relatedProperty)
    {
        IsVisible = relatedProperty.Value?.Equals(Attribute.ShowIfValue) ?? relatedProperty.Value == Attribute.ShowIfValue;
        OnVisibilityChanged?.Invoke();
    }
    
    private void CreateSubProperties()
    {
        SubProperties = new List<PropertyData>();
        if (Value == null) return;
        
        foreach (PropertyInfo property in Value.GetType().GetProperties())
        {
            CustomPropertyAttribute? propertyAttribute = property.GetCustomAttribute<CustomPropertyAttribute>();
            if (propertyAttribute == null) continue;
            SubProperties.Add(new PropertyData(Value, property, propertyAttribute));
        }
        foreach (var property in SubProperties)
        {
            property.InitRelations(SubProperties);
            property.OnValueChangedFromProperties += () => OnValueChangedFromProperties?.Invoke();
            OnValueChangedFromWorld += () => property.OnValueChangedFromWorld?.Invoke();
        }
    }

    public void SetValue(object? value)
    {
        if (value == null) return;
        PropertyInfo.SetValue(Item, value);
        Value = value;
        OnValueChangedFromProperties?.Invoke();
    }
}