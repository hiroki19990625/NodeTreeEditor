using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NodeTreeEditor.Variables;
using NodeTreeEditor.Utils;
using NodeTreeEditor.Window;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Selector.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Selector")]
    public class Selector : Content
    {

        [HideInInspector]
        public List<Conditions> list = new List<Conditions>();

        public override IEnumerator Invoke()
        {
            if (list.Count == 0)
            {
                Debug.LogError("[エラー]条件が設定されていません。");
                yield break;
            }
            foreach (Conditions cond in list)
            {
                if (cond.next == null) continue;
                if (cond.DoConditions())
                {
                    yield return cond.next.Invoke();
                    yield break;
                }
            }
            if (next == null)
            {
                Debug.LogError("[エラー]その他が設定されていません。");
                yield break;
            }
            yield return next.Invoke();
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "条件分岐";
        }

        public override Color WindowColor()
        {
            return Color.green;
        }

        public override void Draw()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                Conditions removeCond = null;
                int c = 0;
                EditorGUILayout.Space();
                foreach (Conditions cond in list)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.LabelField("Condition " + (c + 1));
                        cond.type = (Conditions.CondType)EditorGUILayout.EnumPopup("条件", cond.type);
                        EditorGUILayout.Space();
                        cond.valueTypeA = (Conditions.ValueType)EditorGUILayout.EnumPopup("値A の 種類", cond.valueTypeA);
                        switch (cond.valueTypeA)
                        {

                            case Conditions.ValueType.Raw:
                                cond.SysTypeA = (Value.ValueType)EditorGUILayout.EnumPopup("値A　の タイプ", cond.SysTypeA);

                                switch (cond.SysTypeA)
                                {

                                    case Value.ValueType.Int:
                                        cond.rawInt = EditorGUILayout.IntField("値(int)", cond.rawInt);
                                        break;

                                    case Value.ValueType.Float:
                                        cond.rawFloat = EditorGUILayout.FloatField("値(float)", cond.rawFloat);
                                        break;

                                    case Value.ValueType.Bool:
                                        cond.rawBool = EditorGUILayout.Toggle("値(bool)", cond.rawBool);
                                        break;

                                    case Value.ValueType.String:
                                        cond.rawString = EditorGUILayout.TextField("値(string)", cond.rawString);
                                        break;
                                }
                                break;

                            case Conditions.ValueType.Variable:
                                if (GUILayout.Button("Select ValueA"))
                                {
                                    var n = gameObject.transform.FindChild("LocalVariable");
                                    if (n != null)
                                    {
                                        var values = n.gameObject.GetComponents<Value>();
                                        var menu = new GenericMenu();
                                        var i = 0;
                                        foreach (Value v in values)
                                        {
                                            menu.AddItem(new GUIContent(i + "." + v.valueName), false, GenClickDel, new ArrayList(){
                                            "A",
                                            cond,
                                            v
                                        });
                                            i++;
                                        }
                                        menu.ShowAsContext();
                                    }
                                }
                                if (cond.valueA != null)
                                {
                                    EditorGUILayout.LabelField("リンク済み:  " + cond.valueA.valueName);
                                }
                                else
                                {
                                    EditorGUILayout.HelpBox("条件の接続先が見つかりません。", MessageType.Error);
                                }
                                break;
                        }
                    }

                    if (GUILayout.Button("Select ValueB"))
                    {
                        var n = gameObject.transform.FindChild("LocalVariable");
                        if (n != null)
                        {
                            var values = n.gameObject.GetComponents<Value>();
                            var menu = new GenericMenu();
                            var i = 0;
                            foreach (Value v in values)
                            {
                                menu.AddItem(new GUIContent(i + "." + v.valueName), false, GenClickDel, new ArrayList(){
                                    "B",
                                    cond,
                                    v
                                });
                                i++;
                            }
                            menu.ShowAsContext();
                        }
                    }
                    if (cond.valueB != null)
                    {
                        EditorGUILayout.LabelField("リンク済み:  " + cond.valueB.valueName);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("条件の接続先が見つかりません。", MessageType.Error);
                    }

                    GUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        if (GUILayout.Button("削除"))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "Condition " + (c + 1) + "を削除しますか?", "OK", "キャンセル"))
                            {
                                removeCond = cond;
                            }
                        }
                        if (cond.next != null)
                        {
                            if (GUILayout.Button("リンクを解除"))
                            {
                                if (EditorUtility.DisplayDialog("Warning", "Condition " + (c + 1) + "のリンクを解除しますか?", "OK", "キャンセル"))
                                {
                                    cond.next = null;
                                }
                            }
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                    c++;
                }
                if (removeCond != null)
                {
                    list.Remove(removeCond);
                }
            }
            GUILayout.EndVertical();
            if (GUILayout.Button("条件を追加..."))
            {
                list.Add(new Conditions());
            }
        }

        void GenClickDel(object obj)
        {
            var l = (ArrayList)obj;
            var cmd = (string)l[0];
            if (cmd == "A")
            {
                var cond = (Conditions)l[1];
                cond.valueA = (Value)l[2];
            }
            else
            {
                var cond = (Conditions)l[1];
                cond.valueB = (Value)l[2];
            }
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
                else if (GUILayout.Button("リンク"))
                {
                    window.flag = true;
                    Link(window);
                }
                else if (GUILayout.Button("全てリンク解除"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "このコンテンツのリンクを解除しますか?", "OK", "キャンセル"))
                    {
                        UnLink();
                    }
                }
                else if (GUILayout.Button("その他のリンク解除"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "このコンテンツのリンクを解除しますか?", "OK", "キャンセル"))
                    {
                        next = null;
                    }
                }
            }
        }

        public override void UnLink()
        {
            foreach (Conditions cond in list)
            {
                cond.next = null;
            }
            next = null;
        }

        protected override void ConnectClient(Content content)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Other"), false, GDOther, content);
            int c = 0;

            foreach (Conditions cond in list)
            {
                menu.AddItem(new GUIContent("Condition " + (c + 1)), false, GDConnect, new ArrayList() {
                    c,
                    content,
                    cond
                });
                c++;
            }
            menu.ShowAsContext();
        }

        void GDOther(object obj)
        {
            next = (Content)obj;
        }

        void GDConnect(object obj)
        {
            var al = (ArrayList)obj;
            var index = (int)al[0];
            var content = (Content)al[1];

            list[index].next = content;
        }

        public override Color LineColor()
        {
            return Color.green;
        }

        public override void LineDraw()
        {
            int c = 0;
            foreach (Conditions cond in list)
            {
                if (cond.next != null)
                {
                    ConnectLine(this, cond.next, LineColor(), c + 1);
                }
                c++;
            }
            if (next != null)
            {
                ConnectLine(this, next, Color.black);
            }
        }

        protected virtual void ConnectLine(Content to, Content from, Color color, int index)
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
            
			Handles.DrawSolidRectangleWithOutline (new Vector3[]{(f * (Vector3.down * 10)) + centerP, (f * (Vector3.right * 20)) + centerP, (f * (Vector3.up * 10)) + centerP, (f * (Vector3.down * 10)) + centerP}, color, color);
            //Handles.DrawPolyLine((f * (Vector3.down * 10)) + centerP, (f * (Vector3.right * 20)) + centerP, (f * (Vector3.up * 10)) + centerP, (f * (Vector3.down * 10)) + centerP);
			GUI.Label(new Rect(centerP.x, centerP.y, 200, 20), "Cond" + index);

            Handles.color = Color.black;
        }
#endif
    }
}