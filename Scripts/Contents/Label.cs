using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Label.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Label")]
    public class Label : Content
    {

        [HideInInspector]
        public string label = "Label";

        public override IEnumerator Invoke()
        {
            yield return next.Invoke();
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "ラベル、Gotoノードの移動先。";
        }

        public override Color WindowColor()
        {
            return new Color32(200, 200, 0, 255);
        }

        public override void Draw()
        {
            label = EditorGUILayout.TextField("ラベル", label);
        }

        public override Color LineColor()
        {
            return new Color32(200, 200, 0, 255);
        }
#endif
    }
}