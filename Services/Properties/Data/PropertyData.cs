using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapEditor.Extensions.Reflection;
using MapEditor.Models.Server;
using MapEditor.Services.Properties.Attributes;

namespace MapEditor.Services.Properties.Data;

public class PropertyData
{
    private readonly object Item;
    private readonly PropertyDataEvents DataEvents;
    private readonly CustomPropertyAttribute Attribute;
    
    public string PropertyName => Attribute.PropertyName;
    public string VisualizedName => Attribute.VisualizeName;
    public bool IsReadOnly => Attribute.IsReadOnly;
    public bool IsVisible { get; private set; }
    public object? Value { get; private set; }
    public List<PropertyData>? SubProperties { get; private set; }
    public List<PropertyData>? ItemListProperties { get; private set; }
    public Action? OnValueChangedFromWorld { get; set; }
    public Action? OnValueChangedFromProperties { get; set; }
    public Action? OnVisibilityChanged { get; set; }
    
    public PropertyData(object item, PropertyDataEvents dataEvents, CustomPropertyAttribute attribute)
    {
        Item = item;
        DataEvents = dataEvents;
        Attribute = attribute;
        IsVisible = true;
        
        Value = dataEvents.GetValue();
        OnValueChangedFromWorld += () => Value = dataEvents.GetValue();
        
        SubscribeToChangedFromDTO();
        CreateSubProperties();
        InitListProperties();
    }

    public void InitRelations(IEnumerable<PropertyData> properties)
    {
        if (Attribute.ShowIfProperty == null) return;
        var relatedProperty = properties.FirstOrDefault(x => x.PropertyName == Attribute.ShowIfProperty);
        if (relatedProperty == null) return;
        ChangePropertyVisibility(relatedProperty);
        relatedProperty.OnValueChangedFromProperties += () => ChangePropertyVisibility(relatedProperty);
        relatedProperty.OnValueChangedFromWorld += () => ChangePropertyVisibility(relatedProperty);
    }
    
    public void SetValue(object? value)
    {
        if (value == null) return;
        DataEvents.SetValue(value);
        Value = value;
        OnValueChangedFromProperties?.Invoke();
    }
    
    private void ChangePropertyVisibility(PropertyData relatedProperty)
    {
        IsVisible = relatedProperty.Value?.Equals(Attribute.ShowIfValue) ?? relatedProperty.Value == Attribute.ShowIfValue;
        OnVisibilityChanged?.Invoke();
    }
    
    private void SubscribeToChangedFromDTO()
    {
        if (Item is AItemDTO itemDto)
        {
            itemDto.OnValueChanged += (propertyName) =>
            {
                if (propertyName != Attribute.PropertyName) return;
                OnValueChangedFromWorld?.Invoke();
            };
        }
    }
    
    private void CreateSubProperties()
    {
        SubProperties = new List<PropertyData>();
        if (Value == null) return;
        
        foreach (PropertyInfo property in Value.GetType().GetProperties())
        {
            CustomPropertyAttribute? propertyAttribute = property.GetCustomAttribute<CustomPropertyAttribute>();
            if (propertyAttribute == null) continue;
            SubProperties.Add(new PropertyData(Value, PropertyDataEvents.FromPropertyInfo(Value, property), propertyAttribute));
        }
        foreach (var property in SubProperties)
        {
            property.InitRelations(SubProperties);
            property.OnValueChangedFromProperties += () => OnValueChangedFromProperties?.Invoke();
            OnValueChangedFromWorld += () => property.OnValueChangedFromWorld?.Invoke();
        }
    }
    
    private void InitListProperties()
    {
        if (Value is null or not IEnumerable<object> || Attribute is not CustomPropertyListAttribute listAttribute) return;
        ItemListProperties = new List<PropertyData>();
        
        var array = ((IEnumerable<object>)Value).ToArray();

        for (int i = 0; i < array.Length; i++)
        {
            var dataEvents = PropertyDataEvents.FromList((IEnumerable<object>)Value, i);
            var attribute = Attribute.Clone() as CustomPropertyAttribute;
            attribute.VisualizeName = $"Item [{i}]";
            attribute.IsReadOnly = listAttribute.IsItemReadOnly;
            var itemListProperty = new PropertyData(array[i], dataEvents, attribute);
            ItemListProperties.Add(itemListProperty);
        }
    }
    
    public void AddItemToList()
    {
        if (Value is not IEnumerable<object?> enumer) return;
        var type = enumer.GetType().GetGenericArguments()[0];
        var item = InstanceHelper.GetNewInstance(type);

        // if array, add item to array
        if (Value is Array array)
        {
            var newArray = Array.CreateInstance(type, array.Length + 1);
            for (int i = 0; i < array.Length; i++)
            {
                newArray.SetValue(array.GetValue(i), i);
            }
            newArray.SetValue(item, array.Length);
            SetValue(newArray);
        }
        // if list, add item to list
        else if (Value is IList list)
        {
            list.Add(item);
            SetValue(list);
        }
        
        InitListProperties();
    }
    
    public void RemoveItemFromList(int index)
    {
        if (Value is not IEnumerable<object?> enumer) return;
        var type = enumer.GetType().GetGenericArguments()[0];
        var item = InstanceHelper.GetNewInstance(type);

        // if array, add item to array
        if (Value is Array array)
        {
            var newArray = Array.CreateInstance(type, array.Length - 1);
            for (int i = 0; i < array.Length; i++)
            {
                if (i == index) continue;
                newArray.SetValue(array.GetValue(i), i);
            }
            SetValue(newArray);
        }
        // if list, add item to list
        else if (Value is IList list)
        {
            list.RemoveAt(index);
            SetValue(list);
        }
        
        InitListProperties();
    }
    
}
