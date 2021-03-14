namespace SeaBattle.Application.Contracts.Models
{
    public class ShotModel
    {
        public string Coord { get;  }

        public ShotModel(string coord)
        {
            Coord = coord;
        }
    }
}
