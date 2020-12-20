using System.Collections.Generic;
using System.Linq;
using TNRD.PackageManager.Modules.Abstract;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Modules
{
    public class IncrementVersionModule : VisualElementModule
    {
        public override string Identifier => "net.tnrd.packmanmodule.incrementversion";
        public override string DisplayName => "Increment Version";

        protected override InjectionMethod Method => InjectionMethod.Insert;
        protected override ModuleVisualElement ElementToInject => incrementVersion;
        protected override VisualElement ElementRoot => insertionRoot;
        protected override int InsertIndex => insertionIndex;

        private VisualElement insertionRoot;
        private int insertionIndex;
        private IncrementVersion incrementVersion;

        protected override void OnInitialize()
        {
            incrementVersion = new IncrementVersion();

            VisualElement details = PackageManagerInjectionHelper.InjectedVisualElement.Root.Q("detail");
            List<VisualElement> children = details.Children().ToList();
            VisualElement versionContainer = details.Q(null, "versionContainer");
            insertionRoot = details;
            insertionIndex = children.IndexOf(versionContainer) + 1;
        }

        private class IncrementVersion : InDevelopmentVisualElement
        {
            private readonly Button buttonMajor;
            private readonly Button buttonMinor;
            private readonly Button buttonPatch;
            private readonly Button buttonPreview;

            public IncrementVersion()
            {
                buttonMajor = new Button(IncrementMajor)
                {
                    text = "Major"
                };

                buttonMinor = new Button(IncrementMinor)
                {
                    text = "Minor"
                };

                buttonPatch = new Button(IncrementPatch)
                {
                    text = "Patch"
                };

                buttonPreview = new Button(IncrementPreview)
                {
                    text = "Preview"
                };

                Add(buttonMajor);
                Add(buttonMinor);
                Add(buttonPatch);
                Add(buttonPreview);

                AddToClassList("container");
                AddToClassList("row");
            }

            private void IncrementMajor()
            {
            }

            private void IncrementMinor()
            {
            }

            private void IncrementPatch()
            {
            }

            private void IncrementPreview()
            {
            }
        }
    }
}