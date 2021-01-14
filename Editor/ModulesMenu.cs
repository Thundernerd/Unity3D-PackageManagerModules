using System;
using System.Collections.Generic;
using System.Linq;
using TNRD.Reflectives;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Modules
{
    internal sealed class ModulesMenu : ToolbarMenu
    {
        private const string KEY_ENABLED = "Enabled";
        private readonly Dictionary<string, IPackageManagerModule> modules = new Dictionary<string, IPackageManagerModule>();

        public ModulesMenu()
        {
            SetMenuName();
            SetMenuStyle();

            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract && Utilities.ImplementsOrInherits(x, typeof(IPackageManagerModule)))
                .ToList();

            if (types.Count == 0)
            {
                menu.AppendAction("No modules available", null);
                return;
            }

            foreach (Type type in types)
            {
                IPackageManagerModule module = (IPackageManagerModule) Activator.CreateInstance(type);
                modules.Add(module.Identifier, module);

                menu.AppendAction(module.DisplayName, ToggleModule, GetModuleStatus, module.Identifier);

                module.Initialize();
                if (WasEnabled(module))
                {
                    module.Enable();
                }
            }

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel, TrickleDown.TrickleDown);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel, TrickleDown.TrickleDown);
            CompilationPipeline.compilationStarted += OnStartCompiling;
        }

        private void SetMenuName()
        {
#if UNITY_2019_4_OR_NEWER
            text = "Modules";
#elif UNITY_2019_3_OR_NEWER
            text = "Modules ▾";
#elif UNITY_2019_2_OR_NEWER
            text = "Modules ▾";
#elif UNITY_2019_1_OR_NEWER
            text = "Modules ▾";
#endif
        }

        private void SetMenuStyle()
        {
#if UNITY_2019_1 || UNITY_2019_2
            RemoveFromClassList("unity-toolbar-menu");
            AddToClassList("unity-label");
            AddToClassList("toolbarButton");
            AddToClassList("pulldown");
#endif
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            foreach (KeyValuePair<string, IPackageManagerModule> kvp in modules)
            {
                IPackageManagerModule module = kvp.Value;
                module.Initialize();

                if (WasEnabled(module))
                {
                    module.Enable();
                }
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            DisableAndDisposeModules();
        }

        private void OnStartCompiling(object obj)
        {
            DisableAndDisposeModules();
        }

        private void DisableAndDisposeModules()
        {
            foreach (KeyValuePair<string, IPackageManagerModule> kvp in modules)
            {
                IPackageManagerModule module = kvp.Value;

                if (module.IsEnabled)
                {
                    module.Disable();
                }

                module.Dispose();
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