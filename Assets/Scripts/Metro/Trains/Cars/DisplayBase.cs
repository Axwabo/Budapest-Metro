using Metro.Trains.Routes;
using UnityEngine;

namespace Metro.Trains.Cars
{

    public abstract class DisplayBase<T> : CarComponent where T : DisplayRendererBase
    {

        [SerializeField]
        private MeshRenderer meshRenderer;

        protected override void OnInitialized() => meshRenderer.sharedMaterial = Assembly.RequireComponent<T>().Material;

    }

}
