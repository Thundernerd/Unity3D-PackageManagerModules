using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RectEx;
using TNRD.Constraints;
using TNRD.PackageManager.Modules.Abstract;
using TNRD.PackageManager.Reflected;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace TNRD.PackageManager.Modules
{
    public class DependenciesEditorModule : VisualElementModule
    {
        public override string Identifier => "net.tnrd.packmanmodule.dependencieseditor";
        public override string DisplayName => "Dependencies Editor";

        protected override InjectionMethod Method => InjectionMethod.Add;
        protected override ModuleVisualElement ElementToInject => editButton;
        protected override VisualElement ElementRoot => elementRoot;

        private VisualElement elementRoot;
        private EditButton editButton;

        protected override void OnInitialize()
        {
            editButton = new EditButton();

            elementRoot = PackageManagerInjectionHelper.InjectedVisualElement.PackageDetails.dependencies.dependenciesContainer.parent.Q("dependenciesHeaderLabel");
        }

        private class EditButton : InDevelopmentVisualElement
        {
            private readonly Button buttonToggle;
            private bool enabled;
            private VisualElement dependenciesInnerContainer;
            private VisualElement dependenciesContainer;

            public EditButton()
            {
                style.flexGrow = new StyleFloat(1);

                buttonToggle = new Button(OnToggleClick)
                {
                    text = "Edit"
                };
                buttonToggle.style.alignSelf = new StyleEnum<Align>(Align.FlexEnd);
                buttonToggle.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

                Add(buttonToggle);
            }

            protected override void OnShow()
            {
                parent.style.minHeight = new StyleLength(StyleKeyword.Auto);

                if (dependenciesInnerContainer == null)
                {
                    dependenciesInnerContainer = PackageManagerInjectionHelper.InjectedVisualElement.Root.Q("dependenciesInnerContainer");
                }

                if (dependenciesContainer == null)
                {
                    dependenciesContainer = PackageManagerInjectionHelper.InjectedVisualElement.Root.Q("dependenciesContainer");
                }
            }

            protected override void OnHide()
            {
                parent.style.minHeight = new StyleLength(18f);
            }

            private void OnToggleClick()
            {
                enabled = !enabled;
                dependenciesContainer.style.display = new StyleEnum<DisplayStyle>(enabled ? DisplayStyle.None : DisplayStyle.Flex);

                if (enabled)
                {
                    dependenciesInnerContainer.Add(new DependencyInfoReorderableListVisualElement(Package.dependencies.ToList()));
                }
                else
                {
                    dependenciesInnerContainer.RemoveAt(dependenciesInnerContainer.childCount - 1);
                }
            }
        }

        private class DependencyInfoReorderableListVisualElement : ReorderableListVisualElement<DependencyInfo>
        {
            private string[] displayOptions;

            public DependencyInfoReorderableListVisualElement(List<DependencyInfo> dependencies) : base(dependencies)
            {
                DrawElementCallback += OnDrawElement;
                ReorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;

                displayOptions = PackageDatabase.GetInstance().allPackages
                    .Where(x => x.installedVersion != null)
                    .Select(x => x.installedVersion.name)
                    .OrderBy(x => x)
                    .ToArray();
            }

            private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                Rect[] rects = rect.Column(2);

                DependencyInfo dependencyInfo = Elements[index];

                int popupIndex = 0;

                for (int i = 0; i < displayOptions.Length; i++)
                {
                    string displayOption = displayOptions[i];
                    if (dependencyInfo.name != displayOption)
                        continue;
                    popupIndex = i;
                    break;
                }

                popupIndex = EditorGUI.Popup(Constrain.To(rects[0]).Top.Relative(1).ToRect(), "Name", popupIndex, displayOptions);
                EditorGUI.TextField(Constrain.To(rects[1]).Bottom.Relative(1).ToRect(), "Version", dependencyInfo.version);
            }
        }

        private class ReorderableListVisualElement<T> : IMGUIContainer
        {
            public GUIContent HeaderContent = GUIContent.none;

            protected readonly List<T> Elements;

            protected ReorderableList.HeaderCallbackDelegate DrawHeaderCallback;
            protected ReorderableList.ElementCallbackDelegate DrawElementCallback;
            protected ReorderableList.AddCallbackDelegate AddCallback;
            protected ReorderableList.RemoveCallbackDelegate RemoveCallback;

            protected readonly ReorderableList ReorderableList;

            private bool initialized;

            public ReorderableListVisualElement(List<T> elements = null)
            {
                onGUIHandler = OnGUI;

                Elements = elements ?? new List<T>();
                ReorderableList = new ReorderableList(Elements, typeof(T));
            }

            private void Initialize()
            {
                if (initialized)
                    return;

                if (DrawHeaderCallback != null)
                {
                    ReorderableList.drawHeaderCallback += DrawHeaderCallback;
                }
                else
                {
                    ReorderableList.drawHeaderCallback += OnDrawHeader;
                }

                if (DrawElementCallback != null)
                {
                    ReorderableList.drawElementCallback += DrawElementCallback;
                }

                if (AddCallback != null)
                {
                    ReorderableList.onAddCallback += AddCallback;
                }

                if (RemoveCallback != null)
                {
                    ReorderableList.onRemoveCallback += RemoveCallback;
                }

                initialized = true;
            }

            private void OnGUI()
            {
                if (!initialized)
                {
                    Initialize();
                }

                ReorderableList.DoLayoutList();
            }

            private void OnDrawHeader(Rect rect)
            {
                GUI.Label(rect, HeaderContent);
            }
        }
    }
}