using System.Collections.Generic;
using System.Linq;
using TNRD.PackageManager.Reflected;
using UnityEditor;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Modules
{
    internal static class PackageManagerModulesMenuExtension
    {
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (PackageManagerInjectionHelper.IsAvailable)
            {
                OnInjected();
            }
            else
            {
                PackageManagerInjectionHelper.Injected += OnInjected;
            }
        }

        private static void OnInjected()
        {
            PackageManagerToolbar toolbar = PackageManagerInjectionHelper.InjectedVisualElement.PackageManagerToolbar;
            List<VisualElement> children = toolbar.advancedMenu.parent.Children().ToList();
            int index = children.IndexOf(toolbar.advancedMenu);
            toolbar.advancedMenu.parent.Insert(index, new ModulesMenu());
        }
    }
}