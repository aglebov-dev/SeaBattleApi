using Newtonsoft.Json;

namespace SeaBattle.Api.Contracts
{
    /// <summary>
    /// Represents a data of error.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Contains a message of error.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
