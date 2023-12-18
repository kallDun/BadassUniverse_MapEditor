using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MapEditor.Models.Server;
using MapEditor.Repository;
using MapEditor.Services.Manager;

namespace MapEditor.Services;

public class ApiConnectorService : AService
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    
    private ILoginService loginService;
    private IRepository<WorldDTO> worldRepository;
    public string Token { get; private set; } = string.Empty;
    public string BaseUrl { get; private set; } = "http://localhost:8080";
    
    public ApiConnectorService()
    {
        loginService = new LoginService(this);
        worldRepository = new WorldApiRepository(this);
    }

    public async Task<bool> Login(string username, string password)
    {
        try
        {
            var token = await loginService.Login(username, password);
            Token = token;
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    public Task<bool> CheckBaseUrl() => loginService.CheckConnection();
    public void SetBaseUrl(string baseUrl) => BaseUrl = baseUrl;
    public Task<IEnumerable<WorldDTO>> GetWorlds() => worldRepository.GetAll();
    public Task TryToAddCurrentWorld()
    {
        var world = StorageService.WorldDTO;
        return worldRepository.Add(world);
    }
    public async Task<bool> TryToLoadWorld(int id)
    {
        var world = await worldRepository.Get(id);
        try
        {
            StorageService.SetWorld(world);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}