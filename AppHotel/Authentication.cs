using AppHotel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class Authentication
{
    private readonly HttpClient _httpClient;

    public Authentication()
    {
        _httpClient = new HttpClient();
    }

    public async Task<LoginResponse> AuthenticateUser(string usuario, string senha)
    {
        // Constrói a URL da rota de autenticação da API
        string apiUrl = "http://192.168.1.6:8000/login";

        // Objeto JSON com os dados de login
        var loginData = new { usuario, senha };

        // Envia a requisição POST para a rota de autenticação
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync(apiUrl, loginData);

        LoginResponse response = await DeserializeJsonContentAsync<LoginResponse>(httpResponse);

        return response;

    }

    /// <summary>
    /// Método responsável por desserializar a resposta da requisição.
    /// </summary>
    /// <param name="response">Objeto que será desserializado</param>
    /// <returns>Retorna um objeto Json desserializado</returns>
    public async Task<LoginResponse> DeserializeJsonContentAsync<LoginResponse>(HttpResponseMessage response)
    {
        string jsonContent = await response.Content.ReadAsStringAsync();

        LoginResponse result = JsonConvert.DeserializeObject<LoginResponse>(jsonContent);

        return result;
    }
}
