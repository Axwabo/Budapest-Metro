using UnityEngine.UIElements;

namespace Metro
{

    public static class UIExtensions
    {

        public static void Display(this VisualElement element, bool visible = true) => element.EnableInClassList("display-none", !visible);

    }

}
