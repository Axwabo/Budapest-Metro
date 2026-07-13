using UnityEngine;
using UnityEngine.UIElements;

namespace Metro
{

    public static class UIExtensions
    {

        [RuntimeInitializeOnLoadMethod]
        private static void Init() => ConverterGroups.RegisterGlobalConverter<bool, StyleEnum<Visibility>>((ref bool value) => value ? Visibility.Visible : Visibility.Hidden);

        public static void Display(this VisualElement element, bool visible = true) => element.EnableInClassList("display-none", !visible);

    }

}
