using System.Collections.Generic;
using System.Linq;

namespace MapEditor.Services.Manager
{
    public class ServicesManager
    {
        private static ServicesManager? instance;
        public static ServicesManager Instance => instance ??= new ServicesManager();

        private readonly List<IService> services = new();

        public T GetService<T>() where T : IService, new()
        {
            IService? service = services.FirstOrDefault(service => service is T);
            if (service is null)
            {
                service = new T();
                service.OnDestroy += () => services.Remove(service);
                services.Add(service);
                service.Initialize();
            }
            return (T)service;
        }
        
        public void ResetAllServices()
        {
            services.Clear();
        }

        public void RegisterService<T>() where T : IService, new() => GetService<T>();
    }
}
