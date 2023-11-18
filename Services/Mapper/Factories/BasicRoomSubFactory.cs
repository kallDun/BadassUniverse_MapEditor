using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using System;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicRoomSubFactory : IRoomSubFactory
    {
        Type ISubFactory.SubType => typeof(Room);

        Room IRoomSubFactory.CreateRoom(RoomDTO room)
        {
            throw new NotImplementedException();
        }
    }
}
