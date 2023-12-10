using System;
using System.Collections.Generic;
using System.Reflection;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties.Attributes;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Services.Properties;

public class PropertiesService : AService
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    
    public List<PropertyData> Properties { get; } = new();
    public Action? OnPropertiesChanged { get; set; }
    
    public void SetActiveItem(AItemDTO item)
    {
        Properties.Clear();
        foreach (PropertyInfo property in item.GetType().GetProperties())
        {
            CustomPropertyAttribute? propertyAttribute = property.GetCustomAttribute<CustomPropertyAttribute>();
            if (propertyAttribute == null) continue;
            Properties.Add(new PropertyData(item, PropertyDataEvents.FromPropertyInfo(item, property), propertyAttribute));
        }
        foreach (var property in Properties)
        {
            property.InitRelations(Properties);
        }
        OnPropertiesChanged?.Invoke();

        foreach (var property in Properties)
        {
            property.OnValueChangedFromProperties += () => StorageService.UpdateWorld();
        }
    }
}