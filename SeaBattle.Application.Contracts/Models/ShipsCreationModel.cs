namespace SeaBattle.Application.Contracts.Models
{
    public class ShipsCreationModel
    {
        public string Coordinates { get; }

        public ShipsCreationModel(string coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
