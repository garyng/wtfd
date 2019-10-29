using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wtfd.Models;

namespace Wtfd
{
	public class DocsConverter : JsonConverter<Docs>
	{
		public override Docs Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.StartArray)
			{
				return new Docs
				{
					Descriptions = JsonSerializer.Deserialize<IEnumerable<string>>(ref reader, options)
				};
			}
			else if (reader.TokenType == JsonTokenType.StartObject)
			{
				return new Docs
				{
					NestedDocs = JsonSerializer.Deserialize<Dictionary<string, Docs>>(ref reader, options)
				};
			}

			return null;
		}

		public override void Write(Utf8JsonWriter writer, Docs value, JsonSerializerOptions options)
		{
			if (value.Descriptions != null)
			{
				JsonSerializer.Serialize(writer, value.Descriptions, options);
			}
			else if (value.NestedDocs != null)
			{
				JsonSerializer.Serialize(writer, value.NestedDocs, options);
			}
		}
	}

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