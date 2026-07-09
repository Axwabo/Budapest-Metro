using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Metro.Menu
{

    public sealed class Begin : DocumentComponent
    {

        [SerializeField]
        [HideInInspector]
        private string sceneName;

        protected override void Init(VisualElement root) => root.Q<Button>("Play").clicked += () => SceneManager.LoadScene(sceneName);

#if UNITY_EDITOR
        [SerializeField]
        private SceneAsset scene;

        private void OnValidate() => sceneName = scene?.name;
#endif

    }

}
