using UnityEngine;

namespace Metro
{

    public abstract class Subcomponent<T> : MonoBehaviour
    {

        public T Parent { get; private set; }

        public void Initialize(T parent)
        {
            Parent = parent;
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

    }

}
