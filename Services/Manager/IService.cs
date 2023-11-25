using System;

namespace BadassUniverse_MapEditor.Services.Manager
{
    public interface IService
    {
        Action? OnDestroy { get; set; }

        void Initialize();

        void Reset();

        void Destroy();
    }
}
