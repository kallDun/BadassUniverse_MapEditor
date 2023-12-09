using System;
using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories
{
    public class BasicMobSubFactory : IMobSubFactory
    {
        Mob IMobSubFactory.CreateMob(MobDTO mob)
        {
            throw new NotImplementedException();
        }
    }
}
