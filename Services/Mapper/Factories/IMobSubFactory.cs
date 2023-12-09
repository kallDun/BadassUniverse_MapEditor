using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories
{
    public interface IMobSubFactory : ISubFactory
    {
        Mob CreateMob(MobDTO mob);
    }
}
