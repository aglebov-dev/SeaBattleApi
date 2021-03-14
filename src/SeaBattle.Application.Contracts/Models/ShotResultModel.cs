namespace SeaBattle.Application.Contracts.Models
{
    public class ShotResultModel
    {
        public bool ShipDestroyed { get;  }
        public bool Knock { get; }
        public bool GameEnded { get; }

        public ShotResultModel(bool shipDestroyed, bool knock, bool gameEnded)
        {
            ShipDestroyed = shipDestroyed;
            Knock = knock;
            GameEnded = gameEnded;
        }
    }
}
