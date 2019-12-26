using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Sync.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Sync")]
    public class Sync : Content
    {
        [HideInInspector] public int maxCount = 0;

        [HideInInspector] public int count = 0;

        public override IEnumerator Invoke()
        {
            count++;
            if (maxCount == count)
            {
                Reset();
                yield return next.Invoke();
            }

            yield return null;
        }

        public void Reset()
        {
            count = 0;
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "処理を同期します。";
        }

        public override Color WindowColor()
        {
            return new Color32(0, 100, 255, 255);
        }

        public override void Draw()
        {
            maxCount = EditorGUILayout.IntField("同期終了回数", maxCount);
        }

        public override Color LineColor()
        {
            return new Color32(0, 100, 255, 255);
        }
#endif
    }
}