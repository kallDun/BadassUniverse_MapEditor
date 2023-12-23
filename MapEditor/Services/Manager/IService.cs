using System;

namespace MapEditor.Services.Manager
{
    public interface IService
    {
        Action? OnDestroy { get; set; }

        void Initialize();

        void Reset();

        void Destroy();
    }
}
