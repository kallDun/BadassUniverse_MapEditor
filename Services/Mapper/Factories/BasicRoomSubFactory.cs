using Newtonsoft.Json;
using System;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Models.Game.Rooms;
using MapEditor.Models.Server;
using MapEditor.Services.Storage;
using MapEditor.Services.Storage.Data;

namespace MapEditor.Services.Mapper.Factories;

public class BasicRoomSubFactory : IRoomSubFactory
{
    public Room CreateRoom(RoomDTO roomDTO, IGameStorage storage)
    {
        RoomStorageData data = storage.GetRoomData(roomDTO.InGameRoomId)
                               ?? throw new ArgumentException($"Room with id {roomDTO.InGameRoomId} does not exist.");

        switch (data.RoomType)
        {
            case { } type when type == typeof(AnyFormRoom):
                return new AnyFormRoom(roomDTO.Id, data.Name, roomDTO.Floor, roomDTO.Color, roomDTO.Rotation, roomDTO.State,
                    data.Params as AnyFormBuildingParameters
                    ?? throw new ArgumentException("Wrong format of room parameters."),
                    data.DoorParams as RoomDoorParameters[]
                    ?? throw new ArgumentException("Wrong format of door parameters."));

            case { } type when type == typeof(StreetRoom):
                return new StreetRoom(roomDTO.Id, data.Name, roomDTO.Floor, roomDTO.Color, roomDTO.Rotation, roomDTO.State,
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