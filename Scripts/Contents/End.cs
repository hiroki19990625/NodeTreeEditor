using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using NodeTreeEditor.Window;

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// End.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Base/End")]
    public class End : Content
    {
        public End()
        {
            commonName = "End";
            position = new Rect(400, 0, 0, 0);
        }

        public override IEnumerator Invoke()
        {
            yield return null;
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "エンドポイントです。(終了地点)";
        }

        public override Color WindowColor()
        {
            return new Color32(255, 255, 255, 255);
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
        }
#endif
    }
}