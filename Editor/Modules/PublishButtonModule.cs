using TNRD.PackageManager.Modules.Abstract;
using TNRD.PackageManager.Reflected;
using UnityEngine.UIElements;

namespace TNRD.PackageManager.Modules
{
    public class PublishButtonModule : VisualElementModule
    {
        public override string Identifier => "net.tnrd.packmanmodule.publishbutton";
        public override string DisplayName => "Publish Button";

        protected override InjectionMethod Method => InjectionMethod.Add;
        protected override ModuleVisualElement ElementToInject => publishButton;
        protected override VisualElement ElementRoot => insertionRoot;

        private VisualElement insertionRoot;
        private PublishButton publishButton;

        protected override void OnInitialize()
        {
            publishButton = new PublishButton();
            PackageToolbar packageToolbar = PackageManagerInjectionHelper.InjectedVisualElement.PackageToolbar;
            insertionRoot = packageToolbar.Element.Q("leftItems");
        }

        private class PublishButton : InDevelopmentVisualElement
        {
            private readonly Button buttonPublish;

            public PublishButton()
            {
                buttonPublish = new Button(OnPublishClick)
                {
                    text = "Publish"
                };

                Add(buttonPublish);
            }

            private void OnPublishClick()
            {
            }
        }
    }
}