using UnityEditor;
using UnityEngine;

public sealed class TimeScaleWindow : EditorWindow
{

    private void OnGUI() => Time.timeScale = EditorGUILayout.Slider("Time Scale", Time.timeScale, 0, 100);

    [MenuItem("Metro/Time Scale")]
    private static void ShowWindow()
    {
        var window = GetWindow<TimeScaleWindow>();
        window.titleContent = new GUIContent("Time Scale");
        window.Show();
    }

}
