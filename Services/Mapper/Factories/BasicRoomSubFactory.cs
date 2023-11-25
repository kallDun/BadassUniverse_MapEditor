using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Game.Concrete.Rooms;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Storage;
using BadassUniverse_MapEditor.Services.Storage.Data;
using Newtonsoft.Json;
using System;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicRoomSubFactory : IRoomSubFactory
    {
        public Room CreateRoom(RoomDTO roomDTO, IGameStorage storage)
        {
            RoomStorageData data = storage.GetRoomData(roomDTO.InGameRoomId)
                ?? throw new ArgumentException($"Room with id {roomDTO.InGameRoomId} does not exist.");

            switch (data.RoomType)
            {
                case { } type when type == typeof(SquareRoom):
                    return new SquareRoom(data.Id, data.Name, roomDTO.Floor, roomDTO.Color, roomDTO.Rotation,
                        data.Params as SquareRoomParameters
                            ?? throw new ArgumentException("Wrong format of room parameters."),
                        data.DoorParams as SquareRoomDoorParameters[]
                            ?? throw new ArgumentException("Wrong format of door parameters."));

                case { } type when type == typeof(AnyFormRoom):
                    return new AnyFormRoom(data.Id, data.Name, roomDTO.Floor, roomDTO.Color, roomDTO.Rotation,
                        data.Params as AnyFormRoomParameters
                            ?? throw new ArgumentException("Wrong format of room parameters."),
                        data.DoorParams as SquareRoomDoorParameters[]
                            ?? throw new ArgumentException("Wrong format of door parameters."));

                case { } type when type == typeof(StreetRoom):
                    return new StreetRoom(data.Id, data.Name, roomDTO.Floor, roomDTO.Color, roomDTO.Rotation,
                        JsonConvert.DeserializeObject<StreetRoomParameters>(roomDTO.Params
                            ?? throw new ArgumentNullException("Parameters in room dto must be not null."))
                            ?? throw new ArgumentException("Wrong format of room parameters."),
                        JsonConvert.DeserializeObject<StreetRoomDoorParameters[]>(roomDTO.DoorParams
                            ?? throw new ArgumentNullException("Door prameters in room dto must be not null."))
                            ?? throw new ArgumentException("Wrong format of door parameters."));

                default:
                    throw new ArgumentException($"Room type {data.RoomType} is not supported.");
            }
        }
    }
}
