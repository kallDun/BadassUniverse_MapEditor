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
    private static PreviewService PreviewService
        => ServicesManager.Instance.GetService<PreviewService>();
    
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

    public async Task<IEnumerable<WorldDTO>> GetWorlds()
    {
        try
        {
            var worlds = await worldRepository.GetAll();
            return worlds;
        }
        catch (Exception e)
        {
            return new List<WorldDTO>();
        }
    }
    public async Task TryToAddCurrentWorld()
    {
        try
        {
            var world = StorageService.WorldDTO;
            await worldRepository.Add(world);
        }
        catch (Exception)
        {
            // ignored
        }
    }
    public async Task<bool> TryToLoadWorld(int id)
    {
        var world = await worldRepository.Get(id);
        try
        {
            PreviewService.TryToCancel();
            StorageService.SetWorld(world);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}