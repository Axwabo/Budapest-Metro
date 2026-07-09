using UnityEngine;
using UnityEngine.UIElements;

namespace Metro.Menu
{

    [RequireComponent(typeof(UIDocument))]
    public abstract class DocumentComponent : MonoBehaviour
    {

        private void Start() => Init(GetComponent<UIDocument>().rootVisualElement);

        protected abstract void Init(VisualElement root);

    }

}
