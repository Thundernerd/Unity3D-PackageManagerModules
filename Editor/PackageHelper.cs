using TNRD.PackageManager.Reflected;

namespace TNRD.PackageManager.Modules
{
    public class PackageHelper
    {
        public static IPackage GetCurrentPackage()
        {
            return PackageManagerInjectionHelper.InjectedVisualElement.PackageDetails.package;
        }
    }
}