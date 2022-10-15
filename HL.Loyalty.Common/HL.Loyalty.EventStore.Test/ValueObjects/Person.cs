using Newtonsoft.Json;

namespace HL.Loyalty.EventStore.Test.ValueObjects
{
    internal class Person
    {
        [JsonProperty(PropertyName = "id")]
        public string SomeId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
