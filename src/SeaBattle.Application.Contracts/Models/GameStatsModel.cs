namespace SeaBattle.Application.Contracts.Models
{
    public class GameStatsModel
    {
        public int ShipCount { get; }
        public int Destroyed { get; }
        public int Knocked { get; }
        public int ShotCount { get; }

        public GameStatsModel(int shipCount, int destroyed, int knocked, int shotCount)
        {
            ShipCount = shipCount;
            Destroyed = destroyed;
            Knocked = knocked;
            ShotCount = shotCount;
        }
    }
}
