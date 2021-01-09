using System;
using TNRD.PackageManager.Modules;
using TNRD.PackageManager.Reflected;
using UnityEngine;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Samples.DependenciesEditor
{
    public class DependenciesEditorModule : IPackageManagerModule
    {
        public string Identifier => "net.tnrd.packman.dependencieseditor";
        public string DisplayName => "Dependencies Editor";
        public bool IsEnabled { get; set; }

        private Delegate pageRebuildDelegate;
        private Delegate selectionChangedDelegate;

        private IPageManager pageManager;
        private IPackageVersion currentPackage;

        private DependenciesVisualElement dependenciesVisualElement = new DependenciesVisualElement();

        public void Initialize()
        {
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void Enable()
        {
            pageManager = PageManager.GetInstance();

            Unsubscribe();
            Subscribe();
            UpdatePackage(pageManager.GetSelectedVersion());

            IsEnabled = true;
        }

        public void Disable()
        {
            RemoveDependenciesVisualElement();
            ShowNormalDependencies();
            Unsubscribe();
            IsEnabled = false;
        }

        private void Subscribe()
        {
            pageRebuildDelegate = pageManager.SubscribeToOnPageRebuild((Action<object>) OnPageRebuild);
            selectionChangedDelegate = pageManager.SubscribeToOnSelectionChanged((Action<object>) OnSelectionChanged);
        }

        private void Unsubscribe()
        {
            if (pageRebuildDelegate == null)
                return;

            pageManager.UnsubscribeFromOnPageRebuild(pageRebuildDelegate);
            pageManager.UnsubscribeFromOnSelectionChanged(selectionChangedDelegate);
        }

        private void OnPageRebuild(object pageData)
        {
            UpdatePackage(pageManager.GetSelectedVersion());
        }

        private void OnSelectionChanged(object packageVersionData)
        {
            IPackageVersion packageVersion = new IPackageVersion(packageVersionData);
            UpdatePackage(packageVersion);
        }

        private void UpdatePackage(IPackageVersion packageVersion)
        {
            currentPackage = packageVersion;
            HideNormalDependencies();
            InsertDependenciesVisualElement();
        }

        private void HideNormalDependencies()
        {
            if (!currentPackage.HasTag(PackageTag.InDevelopment))
            {
                ShowNormalDependencies();
                return;
            }

            PackageDependencies dependencies = PackageManagerInjectionHelper.InjectedVisualElement.PackageDetails.dependencies;
            VisualElement visualElement = dependencies.Element.Q("dependenciesContainer");
            visualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        private void ShowNormalDependencies()
        {
            PackageDependencies dependencies = PackageManagerInjectionHelper.InjectedVisualElement.PackageDetails.dependencies;
            VisualElement visualElement = dependencies.Element.Q("dependenciesContainer");
            visualElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        private void InsertDependenciesVisualElement()
        {
            if (!currentPackage.HasTag(PackageTag.InDevelopment))
            {
                RemoveDependenciesVisualElement();
                return;
            }

            PackageDependencies dependencies = PackageManagerInjectionHelper.InjectedVisualElement.PackageDetails.dependencies;
            VisualElement visualElement = dependencies.Element.Q("dependenciesInnerContainer");

            if (!visualElement.Contains(dependenciesVisualElement))
            {
                visualElement.Add(dependenciesVisualElement);
            }

            dependenciesVisualElement.UpdatePackage(currentPackage);
        }

        private void RemoveDependenciesVisualElement()
        {
            PackageDependencies dependencies = PackageManagerInjectionHelper.InjectedVisualElement?.PackageDetails?.dependencies;
            VisualElement visualElement = dependencies?.Element?.Q("dependenciesInnerContainer");
            if (visualElement?.Contains(dependenciesVisualElement) ?? false)
            {
                visualElement?.Remove(dependenciesVisualElement);
            }
        }
    }
}