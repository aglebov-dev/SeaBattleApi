namespace SeaBattle.Domain
{
    public class GameStatsDomainModel
    {
        public int ShipCount { get; }
        public int DestroyedCount { get; }
        public int KnockedCount { get; }
        public int ShotCount { get; }
        public bool IsEnded { get; }

        public GameStatsDomainModel(int shipCount, int destroyed, int knocked, int shotCount, bool isEnded)
        {
            ShipCount = shipCount;
            DestroyedCount = destroyed;
            KnockedCount = knocked;
            ShotCount = shotCount;
            IsEnded = isEnded;
        }
    }
}
