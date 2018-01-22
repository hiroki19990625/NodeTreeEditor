using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using NodeTreeEditor.Window;

using UnityEditor;
#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Setter.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/TODO/Settor")]
    public class Setter : Content
    {
        public override IEnumerator Invoke()
        {
            yield return next.Invoke();
        }

        #if UNITY_EDITOR

        public override string GetDescription()
        {
            return "代入<TODO>";
        }

        public override Color WindowColor()
        {
            return new Color32(60, 130, 130, 255);
        }

        public override Color LineColor()
        {
            return new Color32(60, 130, 130, 255);
        }
        #endif
    }
}