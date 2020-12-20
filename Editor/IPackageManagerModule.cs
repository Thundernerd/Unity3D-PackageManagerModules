namespace TNRD.PackageManager.Modules
{
    public interface IPackageManagerModule
    {
        string Identifier { get; }
        string DisplayName { get; }
        bool IsEnabled { get; }

        void Initialize();
        void Enable();
        void Disable();
    }
}