using UnityEngine;
using System.Collections;

using NodeTreeEditor.Window;

#if UNITY_EDITOR
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
            yield return null;
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "Todo<Setter>";
        }

        public override Color WindowColor()
        {
            return new Color32(255, 100, 200, 255);
        }

        public override Color LineColor()
        {
            return new Color32(255, 100, 200, 255);
        }
#endif
    }
}