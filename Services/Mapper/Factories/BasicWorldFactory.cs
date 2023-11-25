using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using System;
using System.Linq;
using System.Windows;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicWorldFactory : IWorldFactory
    {
        public World CreateWorld(MapDTO mapDto, ISubFactory[] factories)
        {
            App? app = Application.Current as App;
            if (app == null) throw new Exception("App is null");
            string version = app.Version;
            if (mapDto.Version.CompareTo(version) > 0) 
                throw new Exception("Map version is higher than the application version");

            World world = new()
            {
                Name = mapDto.Name,
                Map = Map.InitMap(mapDto.YLenght, mapDto.XLenght)
            };

            if (factories.FirstOrDefault(x => x is IRoomSubFactory) is IRoomSubFactory roomFactory)
            {
                foreach (var roomDto in mapDto.Rooms)
                {
                    Room room = roomFactory.CreateRoom(roomDto);
                    world.Rooms.Add(room);

                    bool result = world.Map.FillWithInnerMap(room.LocalMap, roomDto.Floor, 
                        new MapIndex(roomDto.MapOffsetY, roomDto.MapOffsetX), out Map outMap);
                    if (!result) throw new ArgumentException("Invalid intersection of room map");
                    world.Map = outMap;
                }
            }

            if (factories.FirstOrDefault(x => x is IFacadeSubFactory) is IFacadeSubFactory facadeFactory)
            {
                foreach (var facadeDto in mapDto.Facades)
                {
                    Facade facade = facadeFactory.CreateFacade(facadeDto);
                    world.Facades.Add(facade);

                    bool result = world.Map.FillWithInnerMap(facade.LocalMap, facadeDto.Floor,
                        new MapIndex(facadeDto.MapOffsetY, facadeDto.MapOffsetX), out Map outMap);
                    if (!result) throw new ArgumentException("Invalid intersection of facade map");
                    world.Map = outMap;
                }
            }

            return world;
        }
    }
}
