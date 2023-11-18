using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public interface IMobSubFactory : ISubFactory
    {
        protected abstract Mob CreateMob(MobDTO mob);
    }
}
