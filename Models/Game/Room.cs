using System.Windows.Media;

namespace BadassUniverse_MapEditor.Models.Game
{
    public abstract class Room : WorldElement
    {
        protected Room(int id, string name, int floor, Color color) : base(id, name, floor, color)
        {
        }
    }
}
