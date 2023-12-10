using Newtonsoft.Json;

namespace MapEditor.Models.Server
{
    public class UserDTO
    {
        [JsonProperty("username")] public required string Username { get; set; }

        [JsonProperty("password")] public required string Password { get; set; }
    }
}
