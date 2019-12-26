using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using NodeTreeEditor.Window;
using UnityEditor;

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Goto.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Goto")]
    public class Goto : Content
    {
        [HideInInspector] public string gotoLabel = "Label";

        public override IEnumerator Invoke()
        {
            foreach (var label in GetComponents<Label>())
            {
                if (label.label == gotoLabel)
                {
                    next = label;
                }
            }

            yield return next.Invoke();
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "ラベル、Gotoノードの移動先。";
        }

        public override Color WindowColor()
        {
            return new Color32(255, 100, 65, 255);
        }

        public override void Draw()
        {
            gotoLabel = EditorGUILayout.TextField("移動先", gotoLabel);
        }

        public override void ButtonDraw(NodeEditorWindow window)
        {
            if (window.flag)
            {
                if (GUILayout.Button("接続"))
                {
                    window.flag = false;
                    Connect(window);
                }
                else if (GUILayout.Button("キャンセル"))
                {
                    window.flag = false;
                    Cancel(window);
                }
            }
            else
            {
                if (GUILayout.Button("削除"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "このコンテンツを削除しますか?", "OK", "キャンセル"))
                    {
                        Remove();
                    }
                }
            }
        }

        public override Color LineColor()
        {
            return new Color32(255, 100, 65, 255);
        }
#endif
    }
}