using Newtonsoft.Json;

namespace SeaBattle.Api.Contracts
{
    /// <summary>
    /// Represents the data for creating matrix of new game.
    /// </summary>
    public class MatrixCreationRequest
    {
        /// <summary>
        /// Size of game matrix.
        /// </summary>
        [JsonProperty("range")]
        public int Range { get; set; }
    }
}
