namespace BadassUniverse_MapEditor.Models.Game
{
    public class MapItemWall : MapItem
    {
        public int RelatedRoomIndex { get; set; }

        public MapItemWall(int index)
        {
            RelatedRoomIndex = index;
        }

        public override object Clone()
        {
            return new MapItemWall(RelatedRoomIndex);
        }
        
        public override int GetHashCode()
        {
            return RelatedRoomIndex.GetHashCode() * 13;
        }
    }
}
