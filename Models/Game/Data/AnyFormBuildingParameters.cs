namespace MapEditor.Models.Game.Data
{
    public class AnyFormBuildingParameters
    {
        public int Width { get; set; }
        public int Length { get; set; }

        public required SquareBuildingParameters[] SquareRooms { get; set; }

        public static AnyFormBuildingParameters FromSquareParameters(SquareBuildingParameters parameters)
        {
            return new AnyFormBuildingParameters
            {
                Width = parameters.Width,
                Length = parameters.Length,
                SquareRooms = new[] { parameters }
            };
        }
    }
}
