using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using System;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicRoomSubFactory : IRoomSubFactory
    {
        Type ISubFactory.SubType => throw new NotImplementedException();

        Room IRoomSubFactory.CreateRoom(RoomDTO room)
        {

        }
    }
}
