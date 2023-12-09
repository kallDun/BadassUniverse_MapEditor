using BadassUniverse_MapEditor.Services.Mapper.Factories;
using BadassUniverse_MapEditor.Services.Storage;
using System;
using System.Windows;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public class WorldMapperContextFactory
    {
        public static WorldMapperContext GetContext(string mapVersion)
        {
            string version = GetApplicationVersion();
            if (mapVersion.CompareTo(version) > 0)
                throw new Exception("Map version is higher than the application version");

            WorldMapperContext mapperContext = new(
                mapVersion,
                new TestGameStorage(), 
                new TestListStorage(),
                new BasicWorldFactory(),
                new ISubFactory[]
                {
                    new BasicRoomSubFactory(), new BasicFacadeSubFactory(),
                    new BasicPhysicsItemSubFactory(), new BasicMobSubFactory()
                });

            return mapperContext;
        }

        public static WorldMapperContext GetDefaultContext() => GetContext(GetApplicationVersion());

        private static string GetApplicationVersion()
        {
            App application = Application.Current as App ?? throw new Exception("App is null");
            string version = application.Version;
            return version;
        }
    }
}
