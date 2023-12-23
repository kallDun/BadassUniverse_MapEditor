using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services.Storage;

namespace MapEditor.Services.Mapper.Factories
{
    public interface IRoomSubFactory : ISubFactory
    {
        Room CreateRoom(RoomDTO room, IGameStorage storage);
    }
}
