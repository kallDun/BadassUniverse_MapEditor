using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public abstract class Facade : WorldElement
    {
        protected Facade(int id, string name, int floor, Color color, StoredPreviewState state) 
            : base(id, name, floor, color, state)
        {
        }
    }
}
