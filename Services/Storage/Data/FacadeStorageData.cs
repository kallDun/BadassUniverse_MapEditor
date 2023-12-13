using MapEditor.Models.Game.Data;

namespace MapEditor.Services.Storage.Data;

public class FacadeStorageData
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required AnyFormBuildingParameters Params { get; set; }
}