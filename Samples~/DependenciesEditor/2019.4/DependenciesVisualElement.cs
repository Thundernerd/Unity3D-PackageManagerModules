using System.Collections.Generic;
using System.IO;
using System.Linq;
using TNRD.PackageManager.Reflected;
using TNRD.PackageManifestEditor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Samples.DependenciesEditor
{
    /// <summary>
    /// A visual element that uses IMGUI to draw a Reorderable List
    /// </summary>
    public class DependenciesVisualElement : IMGUIContainer
    {
        private readonly ReorderableList reorderableList;
        private readonly List<Dependency> dependencies = new List<Dependency>();

        private IPackageVersion currentPackage;

        public DependenciesVisualElement()
        {
            onGUIHandler = OnGUI;

            reorderableList = new ReorderableList(dependencies, typeof(Dependency), true, false, true, true);
            reorderableList.drawHeaderCallback += OnDrawHeader;
            reorderableList.drawElementCallback += OnDrawElement;
            reorderableList.onAddDropdownCallback += OnAddDropdown;
        }

        private void OnGUI()
        {
            reorderableList.DoLayoutList();
        }

        private void OnDrawHeader(Rect rect)
        {
            rect.xMin = rect.xMax - 40;
            if (GUI.Button(rect, "Save"))
            {
                string path = currentPackage.packageInfo.assetPath;
                if (path.Contains('/'))
                {
                    path = path.Substring(path.LastIndexOf('/') + 1);
                }

                ManifestEditor manifestEditor = ManifestEditor.Open(path);
                // We're clearing all the dependencies here because we're going to add our own list of dependencies, doing it this way because there is no specific .ClearDependencies function
                foreach (Dependency dependency in manifestEditor.Dependencies)
                {
                    manifestEditor.RemoveDependency(dependency.Id);
                }

                foreach (Dependency dependency in dependencies)
                {
                    manifestEditor.AddDependency(dependency.Id, dependency.Version);
                }

                manifestEditor.Save();
                AssetDatabase.Refresh();
            }
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Dependency dependency = dependencies[index];

            float x = rect.x + 2;
            float y = rect.y + 1;
            float width = rect.width - 4;
            float height = rect.height - 2;
            float halfWidth = width * 0.5f;

            EditorGUI.BeginDisabledGroup(true);
            Rect nameRect = new Rect(x, y, halfWidth - 2, height);
            EditorGUI.TextField(nameRect, dependency.Id);
            EditorGUI.EndDisabledGroup();

            Rect versionRect = new Rect(x + halfWidth + 2, y, halfWidth - 2, height);
            dependency.Version = EditorGUI.TextField(versionRect, dependency.Version);
        }

        private void OnAddDropdown(Rect buttonRect, ReorderableList list)
        {
            IPackageDatabase packageDatabase = PackageDatabase.GetInstance();

            List<IPackageVersion> packages = packageDatabase.upmPackages.Select(x => x.installedVersion)
                .Where(x => x != null)
                .OrderBy(x => x.author ?? "Unknown")
                .ThenBy(x => x.displayName)
                .ToList();

            GenericMenu menu = new GenericMenu();
            foreach (IPackageVersion package in packages)
            {
                GUIContent content = new GUIContent($"{package.author ?? "Unknown"}/{package.displayName}");
                if (dependencies.Any(x => x.Id == package.name))
                {
                    menu.AddItem(content, true, null);
                }
                else
                {
                    menu.AddItem(content, false, AddDependency, package);
                }
            }

            menu.DropDown(buttonRect);
        }

        private void AddDependency(object data)
        {
            IPackageVersion package = (IPackageVersion) data;
            reorderableList.list.Add(new Dependency
            {
                Id = package.name,
                Version = package.versionString
            });
        }

        public void UpdatePackage(IPackageVersion packageVersion)
        {
            currentPackage = packageVersion;
            dependencies.Clear();

            foreach (DependencyInfo dependency in packageVersion.dependencies)
            {
                dependencies.Add(new Dependency
                {
                    Id = dependency.name,
                    Version = dependency.version
                });
            }
        }
    }
}