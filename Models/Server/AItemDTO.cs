using System;
using Newtonsoft.Json;

namespace BadassUniverse_MapEditor.Models.Server;

public abstract class AItemDTO : ICloneable
{
    public Action? OnValuesChanged { get; set; }
    
    [JsonIgnore] public StoredPreviewState State { get; set; } = new StoredPreviewState();
    
    public abstract object Clone();
}