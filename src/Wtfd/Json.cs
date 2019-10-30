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
			switch (reader.TokenType)
			{
				case JsonTokenType.StartArray:
					return new Docs
					{
						Descriptions = JsonSerializer.Deserialize<IEnumerable<string>>(ref reader, options)
					};
				case JsonTokenType.StartObject:
					return new Docs
					{
						NestedDocs = JsonSerializer.Deserialize<Dictionary<string, Docs>>(ref reader, options)
					};
				default:
					return null;
			}
		}

		public override void Write(Utf8JsonWriter writer, Docs value, JsonSerializerOptions options)
		{
			if (value.IsExpanded)
			{
				JsonSerializer.Serialize(writer, value.NestedDocs, options);
			}
			else
			{
				JsonSerializer.Serialize(writer, value.Descriptions, options);
			}
		}
	}

	public static class Json
	{
		public static JsonSerializerOptions Options => new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};

		public static string Serialize<TValue>(TValue value) => JsonSerializer.Serialize(value, Options);
		public static TValue Deserialize<TValue>(string json) => JsonSerializer.Deserialize<TValue>(json, Options);
	}
}