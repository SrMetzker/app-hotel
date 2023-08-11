using Android.App;
using Android.Widget;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class LoginService
{
    private readonly HttpClient _httpClient;

    public LoginService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> AuthenticateUser(string usuario, string senha)
    {
        // Constrói a URL da rota de autenticação da API
        string apiUrl = "http://192.168.1.6:8000/login";

        // Objeto JSON com os dados de login
        var loginData = new { usuario, senha };

        // Envia a requisição POST para a rota de autenticação
        return await _httpClient.PostAsJsonAsync(apiUrl, loginData);

    }
}
