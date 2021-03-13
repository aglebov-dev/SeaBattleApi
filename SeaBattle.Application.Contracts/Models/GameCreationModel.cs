namespace SeaBattle.Application.Contracts.Models
{
    public class GameCreationModel
    {
        public int Size { get; }

        public GameCreationModel(int size)
        {
            Size = size;
        }
    }
}
