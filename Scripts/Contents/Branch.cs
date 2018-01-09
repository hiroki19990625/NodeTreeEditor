using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Branch.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Branch")]
    public class Branch : Content
    {

        [HideInInspector]
        public bool isAsync = false;

        [HideInInspector]
        public List<Content> contents = new List<Content>();

        public override IEnumerator Invoke()
        {
            foreach (var c in contents)
            {
                if (isAsync)
                {
                    StartCoroutine(c.Invoke());
                }
                else
                {
                    yield return c.Invoke();
                }
            }
            yield return null;
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "条件無し分岐。(全て必ずを実行されます。)";
        }

        public override Color WindowColor()
        {
            return new Color32(150, 220, 50, 255);
        }

        public override void Draw()
        {
            isAsync = EditorGUILayout.Toggle("非同期実行", isAsync);
        }

        public override void UnLink()
        {
            contents.Clear();
        }

        protected override void ConnectClient(Content content)
        {
            if (!contents.Contains(content))
            {
                contents.Add(content);
            }
            else
            {
                Debug.LogError("既にリンク済みです。");
            }
        }

        public override Color LineColor()
        {
            return new Color32(150, 220, 50, 255);
        }

        public override void LineDraw()
        {
            foreach (var c in contents)
            {
                ConnectLine(this, c, LineColor());
            }
        }
#endif
    }
}