using System;

namespace BadassUniverse_MapEditor.Services.Manager
{
    public abstract class AService : IService
    {
        public Action? OnDestroy { get; set; }

        public virtual void Initialize() { }

        public virtual void Reset() { }

        public virtual void Destroy()
        {
            OnDestroy?.Invoke();
        }
    }
}
