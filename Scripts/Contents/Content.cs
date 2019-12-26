using System.Collections;
using System.Collections.Generic;
using NodeTreeEditor.Variables;
using UnityEngine;
#if UNITY_EDITOR
using NodeTreeEditor.Window;
using UnityEditor;

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Content.
    /// </summary>
    public abstract class Content : MonoBehaviour
    {
        [SerializeField] protected string commonName = "Node";

        [SerializeField] protected Rect position;

        [HideInInspector] public Vector2 viewPos;

        [HideInInspector] public Content next;

        public abstract IEnumerator Invoke();

        public string GetName()
        {
            return commonName;
        }

        public Rect GetRect()
        {
            return position;
        }

        public void SetRect(Rect position)
        {
            this.position = position;
        }

        public void SetName(string name)
        {
            commonName = name;
        }

        public Content Copy()
        {
            return (Content) MemberwiseClone();
        }

        public List<Value> GetVariables()
        {
            var l = new List<Value>();
            l.AddRange(gameObject.GetComponentsInChildren<Value>());
            return l;
        }

        public void Log(string obj)
        {
            Debug.Log(obj);
        }

#if UNITY_EDITOR

        public abstract string GetDescription();

        public virtual void GetGUIStyle()
        {
            GUI.backgroundColor = WindowColor();
        }

        public virtual Color WindowColor()
        {
            return Color.gray;
        }

        public virtual void Draw()
        {
        }

        public virtual void ButtonDraw(NodeEditorWindow window)
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
                else if (GUILayout.Button("リンク"))
                {
                    window.flag = true;
                    Link(window);
                }
                else if (GUILayout.Button("リンク解除"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "このコンテンツのリンクを解除しますか?", "OK", "キャンセル"))
                    {
                        UnLink();
                    }
                }
            }
        }

        public virtual void Link(NodeEditorWindow window)
        {
            window.linkKeep = this;
        }

        public virtual void UnLink()
        {
            next = null;
        }

        public virtual void Connect(NodeEditorWindow window)
        {
            if (window.linkKeep == this)
            {
                EditorUtility.DisplayDialog("Error", "[エラー]同じノードでループ出来ません。", "OK");
                return;
            }

            window.linkKeep.ConnectClient(this);
        }

        protected virtual void ConnectClient(Content content)
        {
            next = content;
        }

        public virtual void Cancel(NodeEditorWindow window)
        {
            //window.linkKeep.next = null;
        }

        public virtual void Remove()
        {
            DestroyImmediate(this);
        }

        public virtual Color LineColor()
        {
            return Color.black;
        }

        public virtual void LineDraw()
        {
            if (next != null)
            {
                ConnectLine(this, next, LineColor());
            }
        }

        protected virtual void ConnectLine(Content to, Content from, Color color)
        {
            Rect start = to.GetRect();
            Rect end = from.GetRect();
            Vector3 startPos = new Vector3(start.x + (start.width / 2), start.y + (start.height / 2), 0);
            Vector3 endPos = new Vector3(end.x + (end.width / 2), end.y + (end.height / 2), 0);
            Vector3 dif = endPos - startPos;
            float rad = Mathf.Atan2(dif.y, dif.x);

            Quaternion f = Quaternion.Euler(0, 0, rad * (180f / Mathf.PI));

            Vector3 centerP = new Vector3((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2, 0);
            Handles.color = color;
            Handles.DrawLine(startPos, endPos);

            Handles.DrawSolidRectangleWithOutline(
                new Vector3[]
                {
                    (f * (Vector3.down * 10)) + centerP, (f * (Vector3.right * 20)) + centerP,
                    (f * (Vector3.up * 10)) + centerP, (f * (Vector3.down * 10)) + centerP
                }, color, color);
            //Handles.DrawPolyLine((f * (Vector3.down * 10)) + centerP, (f * (Vector3.right * 20)) + centerP, (f * (Vector3.up * 10)) + centerP, (f * (Vector3.down * 10)) + centerP);

            Handles.color = Color.black;
        }
#endif
    }
}