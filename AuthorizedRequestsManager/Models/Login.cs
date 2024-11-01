using System.Text.Json.Serialization;

namespace AuthorizedRequestsManager.Models;

public class Login {
	[JsonPropertyName("login")]
	public string UserLogin { get; set; }
}