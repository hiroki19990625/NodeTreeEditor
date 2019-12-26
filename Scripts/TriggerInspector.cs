#if UNITY_EDITOR
using NodeTreeEditor.Window;
using UnityEditor;
using UnityEngine;

namespace NodeTreeEditor
{
    [CustomEditor(typeof(Trigger))]
    public class TriggerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUIStyle s = new GUIStyle(GUI.skin.button);
            s.fontSize = 15;
            GUI.color = Color.green;
            if (GUILayout.Button(new GUIContent("Open Editor"), s, GUILayout.Height(50)))
            {
                NodeEditorWindow.Open((Trigger) target);
            }

            GUI.color = Color.gray;
        }
    }
}
#endif