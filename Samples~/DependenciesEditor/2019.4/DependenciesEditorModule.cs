using System;
using TNRD.PackageManager.Modules;
using TNRD.PackageManager.Reflected;
using UnityEngine;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Samples.DependenciesEditor
{
    /// <summary>
    /// A module that allows easy editing of a package's dependencies
    /// </summary>
    public class DependenciesEditorModule : IPackageManagerModule
    {
        public string Identifier => "net.tnrd.packman.dependencieseditor";
        public string DisplayName => "Dependencies Editor";
        public bool IsEnabled { get; private set; }

        // The element that will be used to display the dependencies
        private readonly DependenciesVisualElement dependenciesVisualElement = new DependenciesVisualElement();

        /// The delegates used to unsubscribe reflective events
        private Delegate pageRebuildDelegate;
        private Delegate selectionChangedDelegate;

        private IPageManager pageManager;
        private IPackageVersion currentPackage;

        public void Initialize()
        {
            // Nothing to do here
        }

        public void Dispose()
        {
            // Make sure to unsubscribe from the events
            Unsubscribe();
        }

        public void Enable()
        {
            pageManager = PageManager.GetInstance();

            // Unsubscribe first in case we have a lingering subscription
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
            // Subscribing to events here to get notified whenever the selected package changes
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
            // We only want to do this for packages that are in development, this wouldn't work on other packages anyway
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
            // We only want to do this for packages that are in development, this wouldn't work on other packages anyway
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
            try
            {
                PackageDependencies dependencies = PackageManagerInjectionHelper.InjectedVisualElement.PackageDetails.dependencies;
                VisualElement visualElement = dependencies.Element.Q("dependenciesInnerContainer");
                if (visualElement.Contains(dependenciesVisualElement))
                {
                    visualElement.Remove(dependenciesVisualElement);
                }
            }
            catch (NullReferenceException)
            {
                // Thrown when disposing, safe to ignore
            }
        }
    }
}