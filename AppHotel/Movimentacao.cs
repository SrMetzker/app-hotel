using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class Movimentacao
{
    public int Id { get; set; }
    public double Valor { get; set; }
    public DateTime Data { get; set; }
    public string Tipo { get; set; }
    public string Movimento { get; set; }

    private readonly HttpClient _httpClient;

    public Movimentacao()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Método responsável por desserializar a resposta da requisição.
    /// </summary>
    /// <param name="response">Objeto que será desserializado</param>
    /// <returns>Retorna um objeto Json desserializado</returns>
    public async Task<Movimentacao> DeserializeJsonContentAsync<Movimentacao>(HttpResponseMessage response)
    {
        string jsonContent = await response.Content.ReadAsStringAsync();

        Movimentacao mov = JsonConvert.DeserializeObject<Movimentacao>(jsonContent);

        return mov;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Movimentacao> TotalizarEntradas(string hoje)
    {
        // Constrói a URL da rota para a API
        string apiUrl = "http://192.168.1.6:8000/totalizar/entradas?data=" + hoje;

        // Envia a requisição
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(apiUrl);

        Movimentacao mov = await DeserializeJsonContentAsync<Movimentacao>(httpResponse);

        return mov;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Movimentacao> TotalizarSaidas(string hoje)
    {
        // Constrói a URL da rota para a API
        string apiUrl = "http://192.168.1.6:8000/totalizar/saidas?data=" + hoje;

        // Envia a requisição
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(apiUrl);

        Movimentacao mov = await DeserializeJsonContentAsync<Movimentacao>(httpResponse);

        return mov;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<List<Movimentacao>> ObterMovimentacoesData(string data)
    {
        // Constrói a URL da rota para a API
        string apiUrl = "http://192.168.1.6:8000/obter-movimentacoes?data=" + data;

        // Envia a requisição
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(apiUrl);

        List<Movimentacao> mov = await DeserializeJsonContentAsync<List<Movimentacao>>(httpResponse);

        return mov;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Movimentacao> ObterMovimentacoesTotalizar(string data)
    {
        // Constrói a URL da rota para a API
        string apiUrl = "http://192.168.1.6:8000/obter-movimentacoes-totalizar?data=" + data;

        // Envia a requisição
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(apiUrl);

        Movimentacao mov = await DeserializeJsonContentAsync<Movimentacao>(httpResponse);

        return mov;
    }
}
