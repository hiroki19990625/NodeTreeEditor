using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Async.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Async")]
    public class Async : Content
    {
        public override IEnumerator Invoke()
        {
            StartCoroutine(next.Invoke());
            yield return null;
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "非同期実行をします。";
        }

        public override Color WindowColor()
        {
            return new Color32(0, 200, 255, 255);
        }

        public override Color LineColor()
        {
            return new Color32(0, 200, 255, 255);
        }
#endif
    }
}