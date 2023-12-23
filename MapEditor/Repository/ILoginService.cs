using System.Threading.Tasks;

namespace MapEditor.Repository;

public interface ILoginService
{
    Task<string> Login(string username, string password); 
    
    Task<bool> CheckConnection();
}