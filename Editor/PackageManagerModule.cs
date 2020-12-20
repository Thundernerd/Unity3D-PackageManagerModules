namespace TNRD.PackageManager.Modules
{
    public abstract class PackageManagerModule : IPackageManagerModule
    {
        public abstract string Identifier { get; }
        public abstract string DisplayName { get; }
        public bool IsEnabled { get; private set; }

        public void Initialize()
        {
            OnInitialize();
        }

        public void Enable()
        {
            if (IsEnabled)
                return;

            IsEnabled = true;
            OnEnable();
        }

        public void Disable()
        {
            if (!IsEnabled)
                return;

            IsEnabled = false;
            OnDisable();
        }

        protected abstract void OnInitialize();

        protected abstract void OnEnable();

        protected abstract void OnDisable();
    }
}