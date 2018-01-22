using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NodeTreeEditor;
using NodeTreeEditor.Contents;
using NodeTreeEditor.Variables;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


namespace NodeTreeEditor.Window
{
    /// <summary>
    /// Node editor window.
    /// </summary>
    public class NodeEditorWindow : EditorWindow
    {

        const int Editor = 0;
        const int LocalVariable = 1;

        /// <summary>
        /// 
        /// </summary>
        const int windowX = 320;
        const int windowY = 150;

        public static readonly Rect windowSize = new Rect(0, 0, 800, 700);

        public delegate void NodeEditor_Contents_GenericMenuOpenHandler(Trigger tgr,GenericMenu menu);

        public delegate void NodeEditor_Variable_GenericMenuOpenHandler(Trigger tgr,GenericMenu menu);

        public static event NodeEditor_Contents_GenericMenuOpenHandler Contents_GenericMenuOpen;
        public static event NodeEditor_Variable_GenericMenuOpenHandler Variable_GenericMenuOpen;

        public readonly Rect nodeEditorSize = new Rect(0, 0, 2000, 2000);

        public Vector2 scrollView;
        public Vector2 scrollView2;
        public int page = 0;
        public bool flag;

        public bool scrollFlag;

        public Trigger target;

        public Content linkKeep;

        public Dictionary<int, Content> datas = new Dictionary<int, Content>();
        public Dictionary<int, Content> lists = new Dictionary<int, Content>();

        [MenuItem("NodeTreeEditor/NodeEditor")]
        public static void Open()
        {
            GetWindowWithRect<NodeEditorWindow>(windowSize);
        }

        public static void OnVariable_GenericMenuOpen(Trigger tgr, GenericMenu menu)
        {
            if (Variable_GenericMenuOpen != null)
            {
                Variable_GenericMenuOpen(tgr, menu);
            }
        }

        public static void OnContents_GenericMenuOpen(Trigger tgr, GenericMenu menu)
        {
            if (Contents_GenericMenuOpen != null)
            {
                Contents_GenericMenuOpen(tgr, menu);
            }
        }

        void OnGUI()
        {
            target = (Trigger)EditorGUILayout.ObjectField("Trigger", target, typeof(Trigger), true);
            if (target == null)
            {
                EditorGUILayout.HelpBox("Triggerを設定して下さい。", MessageType.Error);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("エディター"))
                {
                    page = 0;
                }
                else if (GUILayout.Button("ローカル変数"))
                {
                    page = 1;
                }
                else if (GUILayout.Button("保存"))
                {
                    Save();
                }
            }

            EditorGUILayout.EndHorizontal();

            if (page == Editor)
            {
                EditorDraw();
            }
            else if (page == LocalVariable)
            {
                LocalVariableDraw();
            }
        }

        void OnDestroy()
        {
            Save();
        }

        void Save()
        {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        void EditorDraw()
        {
            this.wantsMouseMove = true;

            GUI.Box(new Rect(0, 40, this.position.width, this.position.height - 40), "");

            Rect view = new Rect(0, 40, this.position.width, this.position.height - 40);
            scrollView = GUI.BeginScrollView(view, scrollView, nodeEditorSize);
            {
                var ev = Event.current;
                if (ev.type == EventType.ContextClick)
                {
                    var mp = ev.mousePosition;
                    var menu = new GenericMenu();
                    RegisterMenuAll(menu, mp);
                    menu.ShowAsContext();
                    ev.Use();
                }

                /*if (ev.type == EventType.MouseDrag)
                {
                    if (!nodeEditorSize.Contains(ev.mousePosition))
                    {
                        scrollView += ev.delta;
                    }
                }*/
                DrawNodes();
            }
            GUI.EndScrollView();
        }

        void LocalVariableDraw()
        {
            var local = target.transform.FindChild("LocalVariable");
            if (local == null)
            {
                var obj = new GameObject("LocalVariable");
                obj.transform.SetParent(target.transform);
            }

            if (GameObject.Find("VariableList") == null)
            {
                var obj = new GameObject();
                obj.name = "VariableList";
            }

            if (local == null)
                return;

            if (GUILayout.Button("変数を追加..."))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add IntValue"), false, () =>
                    {
                        var c = local.gameObject.AddComponent<IntValue>();
                        c.valueType = Value.ValueType.Int;
                    });
                menu.AddItem(new GUIContent("Add FloatValue"), false, () =>
                    {
                        var c = local.gameObject.AddComponent<FloatValue>();
                        c.valueType = Value.ValueType.Float;
                    });
                menu.AddItem(new GUIContent("Add BoolValue"), false, () =>
                    {
                        var c = local.gameObject.AddComponent<BoolValue>();
                        c.valueType = Value.ValueType.Bool;
                    });
                menu.AddItem(new GUIContent("Add StringValue"), false, () =>
                    {
                        var c = local.gameObject.AddComponent<StringValue>();
                        c.valueType = Value.ValueType.String;
                    });
                OnVariable_GenericMenuOpen(target, menu);
                menu.ShowAsContext();
            }

            scrollView2 = GUILayout.BeginScrollView(scrollView2, GUI.skin.box);
            {
                Value removeV = null;
                foreach (Value v in local.gameObject.GetComponents<Value>())
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        GUILayout.Label(v.valueName + "<" + v.valueType.ToString() + ">");
                        v.valueName = GUILayout.TextField(v.valueName);
                        v.constValue = GUILayout.Toggle(v.constValue, "定数");
                        EditorGUILayout.Space();
                        switch (v.valueType)
                        {
                            case Value.ValueType.Int:
                                var t1 = (IntValue)v;
                                t1.value = EditorGUILayout.IntField("値(int)", t1.value);
                                break;

                            case Value.ValueType.Float:
                                var t2 = (FloatValue)v;
                                t2.value = EditorGUILayout.FloatField("値(float)", t2.value);
                                break;

                            case Value.ValueType.Bool:
                                var t3 = (BoolValue)v;
                                t3.value = EditorGUILayout.Toggle("値(bool)", t3.value);
                                break;

                            case Value.ValueType.String:
                                var t4 = (StringValue)v;
                                t4.value = EditorGUILayout.TextField("値(string)", t4.value);
                                break;
                        }
                        if (GUILayout.Button("削除"))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "この変数を削除しますか?", "OK", "キャンセル"))
                            {
                                removeV = v;
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }
                if (removeV != null)
                {
                    DestroyImmediate(removeV);
                }
            }
            GUILayout.EndScrollView();
        }

        void DrawNodes()
        {
            var contents = target.GetComponents<Content>();

            BeginWindows();
            {
                foreach (Content content in contents)
                {
                    if (content == null)
                        continue;
                    DrawContent(content);
                }
            }
            EndWindows();

            datas.Clear();
        }

        void DrawContent(Content content)
        {
            var r = content.GetRect();
            r.width = windowX;
            r.height = windowY;

            content.LineDraw();

            if (!datas.ContainsValue(content))
            {
                content.GetGUIStyle();
                var wr = GUI.Window(content.GetInstanceID(), r, WindowDraw, content.GetName());
                ResetGUIStyle();

                wr = CheckRect(wr);

                content.SetRect(wr);

                datas.Add(content.GetInstanceID(), content);
                if (!lists.ContainsValue(content))
                {
                    lists.Add(content.GetInstanceID(), content);
                }
            }
        }

        void WindowDraw(int id)
        {
            Content content = lists[id];

            GUILayout.BeginHorizontal();
            {
                content.ButtonDraw(this);
            }
            GUILayout.EndHorizontal();

            content.viewPos = GUILayout.BeginScrollView(content.viewPos);
            {
                EditorGUILayout.HelpBox(content.GetDescription(), MessageType.Info);
                content.SetName(EditorGUILayout.TextField("ContentName", content.GetName()));
                content.Draw();
            }
            GUILayout.EndScrollView();

            GUI.DragWindow();
        }

        Rect CheckRect(Rect rect)
        {
            if (rect.x <= 0)
            {
                rect.position = new Vector2(0, rect.y);
            }
            else if (rect.x >= nodeEditorSize.width - windowX)
            {
                rect.position = new Vector2(nodeEditorSize.width - windowX, rect.y);
            }
            if (rect.y <= 0)
            {
                rect.position = new Vector2(rect.x, 0);
            }
            else if (rect.y >= nodeEditorSize.height - windowY)
            {
                rect.position = new Vector2(rect.x, nodeEditorSize.height - windowY);
            }

            rect.position = new Vector2(Mathf.Round(rect.x / 20) * 20, Mathf.Round(rect.y / 20) * 20);

            return rect;
        }

        void ResetGUIStyle()
        {
            GUI.backgroundColor = Color.white;
        }

        void RegisterMenuAll(GenericMenu menu, Vector2 pos)
        {
            menu.AddItem(new GUIContent("Add Selector"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Selector)
                });
            menu.AddItem(new GUIContent("Add RandomSelector"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(RandomSelector)
                });
            menu.AddItem(new GUIContent("Add Setter"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Setter)
                });
            menu.AddItem(new GUIContent("Add Invoker"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Invoker)
                });
            menu.AddItem(new GUIContent("Add Async"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Async)
                });
            menu.AddItem(new GUIContent("Add Delay"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Delay)
                });
            menu.AddItem(new GUIContent("Add Sync"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Sync)
                });
            menu.AddItem(new GUIContent("Add Branch"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Branch)
                });
            menu.AddItem(new GUIContent("Add Label"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Label)
                });
            menu.AddItem(new GUIContent("Add Goto"), false, AddData, new ArrayList()
                {
                    pos,
                    typeof(Goto)
                });
            OnContents_GenericMenuOpen(target, menu);
        }

        //Del
        void AddData(object obj)
        {
            var list = (ArrayList)obj;
            var pos = (Vector2)list[0];
            var sType = (System.Type)list[1];

            var cn = (Content)target.gameObject.AddComponent(sType);
            cn.SetRect(new Rect(pos, Vector2.zero));
        }
    }
}
#endif