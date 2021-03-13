using Newtonsoft.Json;

namespace SeaBattle.Api.Contracts
{
    /// <summary>
    /// Represents a data of shot.
    /// </summary>
    public class ShotRequest
    {
        /// <summary>
        /// Contains the coordinate at which you need to shoot in the format "1A".
        /// </summary>
        [JsonProperty("coord")]
        public string Coord { get; set; }
    }
}
