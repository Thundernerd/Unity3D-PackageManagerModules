using System;
using TNRD.PackageManager.Reflected;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Modules
{
    public abstract class ModuleVisualElement : VisualElement
    {
        protected IPackageVersion Package { get; private set; }

        private Delegate onSelectionChangedSubscription;

        public void Enable()
        {
            IPageManager pageManager = PageManager.GetInstance();
            onSelectionChangedSubscription = pageManager.SubscribeToOnSelectionChanged((Action<object>) OnSelectionChanged);
            Package = pageManager.GetSelectedVersion();
            OnEnable();
        }

        public void Disable()
        {
            PageManager.GetInstance().UnsubscribeFromOnSelectionChanged(onSelectionChangedSubscription);
            OnDisable();
        }

        private void OnSelectionChanged(object data)
        {
            Package = new IPackageVersion(data);
            OnSelectedPackageChanged(Package);
        }

        protected virtual void OnSelectedPackageChanged(IPackageVersion selectedPackage)
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        public void Show()
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            OnShow();
        }

        public void Hide()
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            OnHide();
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }
    }
}