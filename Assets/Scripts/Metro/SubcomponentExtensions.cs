using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Metro
{

    public static class SubcomponentExtensions
    {

        public static void GetComponentsInImmediateChildren<T>(this Component component, List<T> target)
        {
            var get = ListPool<T>.Get();
            component.GetComponents(get);
            target.AddRange(get);
            var t = component.transform;
            var childCount = t.childCount;
            for (var i = 0; i < childCount; i++)
            {
                t.GetChild(childCount).GetComponents(get);
                target.AddRange(get);
            }

            ListPool<T>.Release(get);
        }

        public static void InitializeComponents<TComponent, TParent>(this TParent parent, List<TComponent> target) where TComponent : Subcomponent<TParent> where TParent : Component
        {
            parent.GetComponentsInImmediateChildren(target);
            foreach (var component in target)
                component.Initialize(parent);
        }

    }

}
