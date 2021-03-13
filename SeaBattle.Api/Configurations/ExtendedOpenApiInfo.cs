using Microsoft.OpenApi.Models;

namespace SeaBattle.Api.Configurations
{
    public class ExtendedOpenApiInfo : OpenApiInfo
    {
        public string Name { get; set; }
        public string UriResource { get; set; }
    }
}
