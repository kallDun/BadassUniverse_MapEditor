using System;

namespace BadassUniverse_MapEditor.Models.Game
{
    public struct MapIndex
    {
        public MapIndex() { }

        public MapIndex(int y, int x)
        {
            Y = y;
            X = x;
        }

        public int Y { get; set; }
        public int X { get; set; }


        public static bool operator == (MapIndex left, MapIndex right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator != (MapIndex left, MapIndex right)
        {
            return left != right;
        }
        
        public static MapIndex operator -(MapIndex left, MapIndex right)
        {
            return new MapIndex(left.Y - right.Y, left.X - right.X);
        }

        public static MapIndex operator +(MapIndex left, MapIndex right)
        {
            return new MapIndex(left.Y + right.Y, left.X + right.X);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Y, X);
        }

        public override readonly bool Equals(object obj)
        {
            return obj is MapIndex index && this == index;
        }
    }
}
