using System.Text.Json;
using AuthorizedRequestsManager.Models;

namespace AuthorizedRequestsManager;

public class AuthorizedRequestsManager {
	private readonly HttpClientHandler clientHandler = new();

	public AuthorizedRequestsManager() {
		clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
	}

	private string UserLogin { get; set; }

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

	public async Task<bool> Login(string Login, string Password) {
		using (var client = new HttpClient(clientHandler)) {
			var response = await client.PostAsync
				(
				 "https://localhost:7155/api/auth/sign_in/",
				 new FormUrlEncodedContent(new Dictionary<string, string> { { "Login", Login }, { "Password", Password } })
				);

			if (!response.IsSuccessStatusCode) return false;

			TokensPair = await JsonSerializer.DeserializeAsync<Tokens>(await response.Content.ReadAsStreamAsync());
			UserLogin = Login;

			return true;
		}
	}

	public async Task<bool> Logout() {
		using (var client = new HttpClient(clientHandler)) {
			var response = await client.PostAsync
				(
				 "https://localhost:7155/api/auth/sign_out/",
				 new FormUrlEncodedContent(new Dictionary<string, string> { { "AccessToken", TokensPair.AccessToken } })
				);

			if (!response.IsSuccessStatusCode) return false;

			TokensPair = null;
			UserLogin = null;

			return true;
		}
	}
}