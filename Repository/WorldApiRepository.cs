using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MapEditor.Models.Server;
using MapEditor.Services;
using Newtonsoft.Json;

namespace MapEditor.Repository;

public class WorldApiRepository : IRepository<WorldDTO>
{
    private readonly ApiConnectorService apiConnectorService;

    public WorldApiRepository(ApiConnectorService apiConnectorService)
    {
        this.apiConnectorService = apiConnectorService;
    }

    public async Task Add(WorldDTO item)
    {
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiConnectorService.Token}");
        
        var content = JsonConvert.SerializeObject(item);
        HttpContent httpContent = new StringContent(content);
        var httpResponse = await httpClient.PostAsync($"{apiConnectorService.BaseUrl}/api/map", httpContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Error while adding world");
        }
    }

    public async Task<WorldDTO> Get(int id)
    {
        HttpClient httpClient = new();
        var httpResponse = await httpClient.GetAsync($"{apiConnectorService.BaseUrl}/api/map/{id}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Error while getting world");
        }
        var content = await httpResponse.Content.ReadAsStringAsync();
        WorldDTO? world = JsonConvert.DeserializeObject<WorldDTO>(content);
        if (world == null)
        {
            throw new Exception("Error while getting world");
        }
        return world;
    }

    public async Task<IEnumerable<WorldDTO>> GetAll()
    {
        HttpClient httpClient = new();
        var httpResponse = await httpClient.GetAsync($"{apiConnectorService.BaseUrl}/api/map");
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Error while getting worlds");
        }
        var content = await httpResponse.Content.ReadAsStringAsync();
        IEnumerable<WorldDTO>? worlds = JsonConvert.DeserializeObject<IEnumerable<WorldDTO>>(content);
        if (worlds == null)
        {
            throw new Exception("Error while getting worlds");
        }
        return worlds;
    }
    
    public Task Remove(WorldDTO item)
    {
        throw new NotImplementedException();
    }

    public Task Update(WorldDTO item)
    {
        throw new NotImplementedException();
    }
}