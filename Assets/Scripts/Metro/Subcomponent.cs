using System;
using UnityEngine;

namespace Metro
{

    public abstract class Subcomponent<T> : MonoBehaviour
    {

        public T Parent { get; private set; }

        public void Initialize(T parent)
        {
            Parent = parent;
            try
            {
                OnInitialized();
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        protected virtual void OnInitialized()
        {
        }

    }

}
