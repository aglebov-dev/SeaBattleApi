using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SeaBattle.Api.Extensions
{
    public static class JsonExtensions
    {
        public static JsonSerializerSettings ApplyDefaultJsonSettings(this JsonSerializerSettings settings)
        {
            settings ??= new JsonSerializerSettings();
            CamelCaseNamingStrategy strategy = new CamelCaseNamingStrategy(processDictionaryKeys: true, overrideSpecifiedNames: false);
            StringEnumConverter stringEnumConverter = new StringEnumConverter(strategy);

            if (settings.ContractResolver is DefaultContractResolver resolver)
            {
                resolver.NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true
                };
            }

            settings.Converters.Add(stringEnumConverter);
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Populate;
            settings.Formatting = Formatting.Indented;

            return settings;
        }
    }
}
