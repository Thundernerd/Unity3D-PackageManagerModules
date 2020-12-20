using TNRD.PackageManager.Reflected;

namespace TNRD.PackageManager.Modules
{
    public class InDevelopmentVisualElement : ModuleVisualElement
    {
        protected bool IsInDevelopment => Package.HasTag(PackageTag.InDevelopment);

        protected override void OnEnable()
        {
            UpdateVisibility();
        }

        protected override void OnSelectedPackageChanged(IPackageVersion selectedPackage)
        {
            UpdateVisibility();
        }

        protected override void OnDisable()
        {
        }

        protected void UpdateVisibility()
        {
            if (IsInDevelopment)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}