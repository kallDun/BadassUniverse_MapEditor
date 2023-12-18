using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MapEditor.Services;
using Newtonsoft.Json;

namespace MapEditor.Repository;

public class LoginService : ILoginService
{
    private class Token
    {
        public string token { get; set; }
    }
    
    private readonly ApiConnectorService apiConnectorService;

    public LoginService(ApiConnectorService apiConnectorService)
    {
        this.apiConnectorService = apiConnectorService;
    }

    public async Task<string> Login(string username, string password)
    {
        HttpClient httpClient = new();
        var user = new { username, password };
        var content = JsonConvert.SerializeObject(user);
        HttpContent httpContent = new StringContent(content, new MediaTypeHeaderValue("application/json"));
        var httpResponse = await httpClient.PostAsync($"{apiConnectorService.BaseUrl}/auth", httpContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Error while logging in");
        }
        var tokenData = JsonConvert.DeserializeObject<Token>(await httpResponse.Content.ReadAsStringAsync());
        if (tokenData == null)
        {
            throw new Exception("Error while logging in");
        }
        return tokenData.token;
    }
    
    public async Task<bool> CheckConnection()
    {
        try
        {
            HttpClient httpClient = new();
            var httpResponse = await httpClient.GetAsync($"{apiConnectorService.BaseUrl}/api/check");
            return httpResponse.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}