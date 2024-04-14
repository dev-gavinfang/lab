using System.Text.Json.Serialization;

namespace AsyncEnumerableSample.Client.Contracts;

public class SampleDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
