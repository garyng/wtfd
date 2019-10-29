using System.Text.Json;

namespace Wtfd
{
	public static class Json
	{
		public static JsonSerializerOptions Options => new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		public static string Serialize<TValue>(TValue value) => JsonSerializer.Serialize(value, Options);
		public static TValue Deserialize<TValue>(string json) => JsonSerializer.Deserialize<TValue>(json, Options);
	}
}