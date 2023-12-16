using System;
using System.Collections.Generic;
using System.Reflection;
using MapEditor.Extensions.Models;
using MapEditor.Models;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties.Attributes;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Services.Properties;

public class PropertiesService : AService
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    private static PreviewService PreviewService
        => ServicesManager.Instance.GetService<PreviewService>();
    
    public List<PropertyData> Properties { get; } = new();
    public List<ActionButtonData> ActionButtons { get; } = new();
    public Action? OnPropertiesChanged { get; set; }
    
    public override void Reset()
    {
        Properties.Clear();
        ActionButtons.Clear();
        OnPropertiesChanged?.Invoke();
    }
    
    public void SetActiveItem(AItemDTO item)
    {
        InitializeProperties(item);
        InitializeButtons(item);
        SubscribeToValueChangedFromProperties(item);
        OnPropertiesChanged?.Invoke();
    }

    private void SubscribeToValueChangedFromProperties(AItemDTO item)
    {
        if (item.State == StoredPreviewState.Preview)
        {
            foreach (var property in Properties)
            {
                property.OnValueChangedFromProperties += () => StorageService.UpdateWorld();
            }
        }
        else if (item.State == StoredPreviewState.Stored)
        {
            foreach (var property in Properties)
            {
                property.OnValueChangedFromProperties += () => { ChangeEnabledForButtons(true); };
            }
        }
    }

    private void ChangeEnabledForButtons(bool isEnabled)
    {
        var revertButton = ActionButtons.Find(x => x.Name == "Revert changes");
        if (revertButton != null)
        {
            revertButton.IsEnabled = isEnabled;
            revertButton.OnEnabledChanged?.Invoke();
        }
        var saveButton = ActionButtons.Find(x => x.Name == "Try to save");
        if (saveButton != null)
        {
            saveButton.IsEnabled = isEnabled;
            saveButton.OnEnabledChanged?.Invoke();
        }
    }

    private void InitializeButtons(AItemDTO item)
    {
        ActionButtons.Clear();
        if (item.State == StoredPreviewState.Preview)
        {
            ActionButtons.Add(
                new ActionButtonData()
                {
                    Name = "Delete preview",
                    ButtonName = "Delete",
                    OnClick = () =>
                    {
                        PreviewService.TryToCancel();
                    }
                });
            ActionButtons.Add(
                new ActionButtonData()
                {
                    Name = "Save preview",
                    ButtonName = "Save",
                    OnClick = () =>
                    {
                        PreviewService.TryToSave();
                    }
                });
        }
        else if (item.State == StoredPreviewState.Stored)
        {
            ActionButtons.Add(
                new ActionButtonData()
                {
                    Name = "Try to save",
                    ButtonName = "Save",
                    IsEnabled = false,
                    OnClick = () =>
                    {
                        var worldDto = StorageService.WorldDTO.Clone() as WorldDTO;
                        if (worldDto == null) return;
                        try
                        {
                            foreach (var property in Properties)
                            {
                                property.SetValueOnClick();
                            }
                            StorageService.UpdateWorld();
                            ChangeEnabledForButtons(false);
                        }
                        catch (Exception e)
                        {
                            StorageService.SetWorld(worldDto);
                            var newItem = worldDto.FindItem(item);
                            if (newItem != null) SetActiveItem(newItem);
                            else Reset();
                        }
                    }
                });
            ActionButtons.Add(
                new ActionButtonData()
                {
                    Name = "Revert changes",
                    ButtonName = "Revert",
                    IsEnabled = false,
                    OnClick = () =>
                    {
                        SetActiveItem(item);
                    }
                });
            if (item is not WorldDTO)
            {
                ActionButtons.Add(
                    new ActionButtonData()
                    {
                        Name = "Remove item",
                        ButtonName = "Remove",
                        OnClick = () =>
                        {
                            StorageService.WorldDTO.RemoveItem(item);
                            StorageService.UpdateWorld();
                            Reset();
                        }
                    });
            }
        }
    }

    private void InitializeProperties(AItemDTO item)
    {
        Properties.Clear();
        foreach (PropertyInfo property in item.GetType().GetProperties())
        {
            CustomPropertyAttribute? propertyAttribute = property.GetCustomAttribute<CustomPropertyAttribute>();
            if (propertyAttribute == null) continue;
            var savingType = item.State switch
            {
                StoredPreviewState.Preview => PropertySavingType.Immediate,
                StoredPreviewState.Stored => PropertySavingType.OnClick,
                _ => throw new ArgumentOutOfRangeException()
            };
            Properties.Add(new PropertyData(item, PropertyDataEvents.FromPropertyInfo(item, property), propertyAttribute, savingType));
        }
        foreach (var property in Properties)
        {
            property.InitRelations(Properties);
        }
    }
}