using System;

namespace MapEditor.Services.Storage.Data
{
    public class RoomStorageData
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public object? Params { get; set; }
        public object? DoorParams { get; set; }
        public required Type RoomType { get; set; }
    }
}
