using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using System;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicMobSubFactory : IMobSubFactory
    {
        Type ISubFactory.SubType => typeof(Mob);

        Mob IMobSubFactory.CreateMob(MobDTO mob)
        {
            throw new NotImplementedException();
        }
    }
}
