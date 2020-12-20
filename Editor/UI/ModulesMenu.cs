using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Modules
{
    sealed internal class ModulesMenu : ToolbarMenu
    {
        private const string KEY_ENABLED = "Enabled";
        private readonly Dictionary<string, IPackageManagerModule> modules = new Dictionary<string, IPackageManagerModule>();

        public ModulesMenu()
        {
            text = "Modules";

            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .ToList();

            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(PackageManagerModule)))
                    continue;
                if (type.IsAbstract)
                    continue;

                IPackageManagerModule module = (IPackageManagerModule) Activator.CreateInstance(type);
                modules.Add(module.Identifier, module);
                menu.AppendAction(module.DisplayName, ToggleModule, GetModuleStatus, module.Identifier);
                module.Initialize();
                if (WasEnabled(module))
                {
                    module.Enable();
                }
            }
        }

        private void ToggleModule(DropdownMenuAction dropdownMenuAction)
        {
            IPackageManagerModule module = modules[(string) dropdownMenuAction.userData];

            if (module.IsEnabled)
            {
                module.Disable();
            }
            else
            {
                module.Enable();
            }

            EditorPrefs.SetBool(GetKey(module, KEY_ENABLED), module.IsEnabled);
        }

        private DropdownMenuAction.Status GetModuleStatus(DropdownMenuAction dropdownMenuAction)
        {
            IPackageManagerModule module = modules[(string) dropdownMenuAction.userData];
            return module.IsEnabled ? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal;
        }

        private bool WasEnabled(IPackageManagerModule module)
        {
            return EditorPrefs.GetBool(GetKey(module, KEY_ENABLED), false);
        }

        private string GetKey(IPackageManagerModule module, string key)
        {
            return $"TNRD.PackageManagerModules.Module.{module.Identifier}.{key}";
        }
    }
}