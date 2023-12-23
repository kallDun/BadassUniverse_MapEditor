using System;

namespace MapEditor.Services.Properties.Data;

public class ActionButtonData
{
    public required string Name { get; set; }
    public required string ButtonName { get; set; }
    public Action? OnClick { get; set; }
    public Action? OnEnabledChanged { get; set; }
    public bool IsEnabled { get; set; } = true;
}