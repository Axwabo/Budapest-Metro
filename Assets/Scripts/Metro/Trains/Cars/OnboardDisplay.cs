using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Trains.Cars
{

    public sealed class OnboardDisplay : CarComponent
    {

        [SerializeField]
        private UIDocument document;

        protected override void OnInitialized() => document.rootVisualElement.style.backgroundImage = Background.FromRenderTexture(Assembly.DisplayRenderer.Texture);

    }

}
