namespace MapEditor.Models.Game
{
    public class MapItemBuilding : MapItem
    {
        public int Index { get; set; }

        public MapItemBuilding(int index)
        {
            Index = index;
        }

        public override object Clone()
        {
            return new MapItemBuilding(Index);
        }
        
        public override int GetHashCode()
        {
            return 13 + Index.GetHashCode() * 31;
        }
    }
}
