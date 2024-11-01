using System.Text.Json;
using AuthorizedRequestsManager.Models;

namespace AuthorizedRequestsManager;

public class AuthorizedRequestsManager {
	private readonly HttpClientHandler clientHandler = new();

	private string UserLogin;

	public AuthorizedRequestsManager() {
		clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
	}

	private Tokens TokensPair { get; set; }

	public async Task<bool> Register(string Login, string Password, string Name) {
		using (var client = new HttpClient(clientHandler)) {
			var response = await client.PostAsync
				(
				 "https://localhost:7155/api/auth/sign_up/",
				 new FormUrlEncodedContent(new Dictionary<string, string> { { "Login", Login }, { "Password", Password }, { "Name", Name } })
				);

			if (!response.IsSuccessStatusCode) return false;

			TokensPair = await JsonSerializer.DeserializeAsync<Tokens>(await response.Content.ReadAsStreamAsync());
			UserLogin = Login;

			return true;
		}
	}
}