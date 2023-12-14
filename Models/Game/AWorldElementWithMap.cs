using System;
using System.Windows.Media;

namespace MapEditor.Models.Game;

public abstract class AWorldElementWithMap : AWorldElement
{
    protected AWorldElementWithMap(int id, string name, MapIndex leftTopCorner, int floor, Color color, StoredPreviewState state) 
        : base(id, name, leftTopCorner, floor, color, state) { }
    
    private Map? map;
    public Map LocalMap => map ??= GenerateMap();
    protected abstract Map GenerateMap();

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, LeftTopCorner, Color, Floor, State, LocalMap);
    }
}