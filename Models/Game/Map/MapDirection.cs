namespace BadassUniverse_MapEditor.Models.Game
{
    public enum MapDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    public static class MapDirectionExtensions
    {
        public static MapDirection Add(this MapDirection Direction, MapDirection Value2)
        {
            return (MapDirection)(((int)Direction + (int)Value2) % 4);
        }

        public static MapDirection Subtract(this MapDirection Direction, MapDirection Value2)
        {
            return (MapDirection)((4 + (int)Direction - (int)Value2) % 4);
        }
    }
}
