using Newtonsoft.Json;

namespace SeaBattle.Api.Contracts
{
    /// <summary>
    /// Represents a data of player's ships.
    /// </summary>
    public class ShipRequest
    {
        /// <summary>
        /// Contains the coordinates of the ships in the following format "1A 2B, 3D 3E".
        /// </summary>
        [JsonProperty("Coordinates")]
        public string Coordinates { get; set; }
    }
}
