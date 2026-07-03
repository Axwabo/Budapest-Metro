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
                t.GetChild(i).GetComponents(get);
                target.AddRange(get);
            }

            ListPool<T>.Release(get);
        }

        public static void InitializeComponents<TParent, TComponent>(this TParent parent, List<TComponent> target) where TParent : Component where TComponent : Subcomponent<TParent>
        {
            foreach (var component in target)
                component.Initialize(parent);
        }

        public static void GetAndInitializeComponents<TParent, TComponent>(this TParent parent, List<TComponent> target) where TParent : Component where TComponent : Subcomponent<TParent>
        {
            parent.GetComponentsInChildren(target);
            parent.InitializeComponents(target);
        }

    }

}
