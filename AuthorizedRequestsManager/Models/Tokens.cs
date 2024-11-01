using System.Text.Json.Serialization;

namespace AuthorizedRequestsManager.Models;

public class Tokens {
	[JsonPropertyName("login")]
	public string AccessToken { get; set; }

	[JsonPropertyName("login")]
	public string RefreshToken { get; set; }
}