using Newtonsoft.Json;

namespace SeaBattle.Api.Contracts
{
    /// <summary>
    /// Represents a statistic of game.
    /// </summary>
    public class StatsResponse
    {
        /// <summary>
        /// Number of ships in the game. 
        /// </summary>
        [JsonProperty("ship_count")]
        public int ShipsCount { get; set; }

        /// <summary>
        /// Number of destroyed ships in the game.
        /// </summary>
        [JsonProperty("destroyed")]
        public int Destroyed { get; set; }

        /// <summary>
        /// Number of knocked ships in the game.
        /// </summary>
        [JsonProperty("knocked")]
        public int Knocked { get; set; }

        /// <summary>
        /// Number of shots.
        /// </summary>
        [JsonProperty("shot_count")]
        public int ShotsCount { get; set; }
    }
}
