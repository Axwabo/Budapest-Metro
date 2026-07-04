using Metro.Trains.Routes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Trains.Cars
{

    public abstract class DisplayBase<T> : CarComponent where T : DisplayRendererBase
    {

        [SerializeField]
        private UIDocument document;

        protected override void OnInitialized() => document.rootVisualElement.style.backgroundImage = Background.FromRenderTexture(Assembly.RequireComponent<T>().Texture);

    }

}
