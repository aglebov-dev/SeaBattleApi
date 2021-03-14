using Newtonsoft.Json;

namespace SeaBattle.Api.Contracts
{
    /// <summary>
    /// Represents a data of shot result.
    /// </summary>
    public class ShotResponse
    {
        /// <summary>
        /// Indicates that the enemy ship are flooded.
        /// </summary>
        [JsonProperty("destroy")]
        public bool Destroy { get; set; }

        /// <summary>
        /// Indicates hitting a ship.
        /// </summary>
        [JsonProperty("knock")]
        public bool Knock { get; set; }

        /// <summary>
        /// Indicates the end of the game. All enemy ships are flooded.
        /// </summary>
        [JsonProperty("end")]
        public bool End { get; set; }
    }
}
