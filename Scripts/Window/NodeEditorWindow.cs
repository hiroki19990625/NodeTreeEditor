#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using NodeTreeEditor.Contents;
using NodeTreeEditor.Variables;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NodeTreeEditor.Window
{
    /// <summary>
    /// Node editor window.
    /// </summary>
    public class NodeEditorWindow : EditorWindow
    {
        const int Editor = 0;
        const int LocalVariable = 1;

        public static readonly Rect windowSize = new Rect(0, 0, 800, 700);

        public delegate void NodeEditor_Contents_GenericMenuOpenHandler(Trigger tgr, GenericMenu menu);

        public delegate void NodeEditor_Variable_GenericMenuOpenHandler(Trigger tgr, GenericMenu menu);

        public static event NodeEditor_Contents_GenericMenuOpenHandler Contents_GenericMenuOpen;
        public static event NodeEditor_Variable_GenericMenuOpenHandler Variable_GenericMenuOpen;

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

        public static void Open(Trigger trigger)
        {
            NodeEditorWindow w = GetWindowWithRect<NodeEditorWindow>(windowSize);
            w.target = trigger;
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
            target = (Trigger) EditorGUILayout.ObjectField("Trigger", target, typeof(Trigger), true);
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
            scrollView = GUI.BeginScrollView(view, scrollView, target.nodeEditorSize);
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

                //TODO: Drag Event...

                DrawNodes();
            }
            GUI.EndScrollView();
        }

        void LocalVariableDraw()
        {
            var local = target.transform.Find("LocalVariable");
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
                menu.AddItem(new GUIContent("Add ObjectValue"), false, () =>
                {
                    var c = local.gameObject.AddComponent<ObjectValue>();
                    c.valueType = Value.ValueType.Object;
                });
                menu.AddItem(new GUIContent("Add GameObjectValue"), false, () =>
                {
                    var c = local.gameObject.AddComponent<GameObjectValue>();
                    c.valueType = Value.ValueType.GameObject;
                });
                menu.AddItem(new GUIContent("Add Vector2Value"), false, () =>
                {
                    var c = local.gameObject.AddComponent<Vector2Value>();
                    c.valueType = Value.ValueType.Vector2;
                });
                menu.AddItem(new GUIContent("Add Vector3Value"), false, () =>
                {
                    var c = local.gameObject.AddComponent<Vector3Value>();
                    c.valueType = Value.ValueType.Vector3;
                });
                menu.AddItem(new GUIContent("Add Vector4Value"), false, () =>
                {
                    var c = local.gameObject.AddComponent<Vector4Value>();
                    c.valueType = Value.ValueType.Vector4;
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
                                var t1 = (IntValue) v;
                                t1.value = EditorGUILayout.IntField("値(int)", t1.value);
                                break;

                            case Value.ValueType.Float:
                                var t2 = (FloatValue) v;
                                t2.value = EditorGUILayout.FloatField("値(float)", t2.value);
                                break;

                            case Value.ValueType.Bool:
                                var t3 = (BoolValue) v;
                                t3.value = EditorGUILayout.Toggle("値(bool)", t3.value);
                                break;

                            case Value.ValueType.String:
                                var t4 = (StringValue) v;
                                EditorGUILayout.LabelField("値(string)");
                                t4.value = EditorGUILayout.TextArea(t4.value);
                                break;

                            case Value.ValueType.Object:
                                var t5 = (ObjectValue) v;
                                t5.value = EditorGUILayout.ObjectField("値(UnityEngine.Object)", t5.value,
                                    typeof(Object), true);
                                break;

                            case Value.ValueType.Vector2:
                                var t7 = (Vector2Value) v;
                                t7.value = EditorGUILayout.Vector2Field("値(Vector2)", t7.value);
                                break;

                            case Value.ValueType.Vector3:
                                var t8 = (Vector3Value) v;
                                t8.value = EditorGUILayout.Vector3Field("値(Vector3)", t8.value);
                                break;

                            case Value.ValueType.Vector4:
                                var t9 = (Vector4Value) v;
                                t9.value = EditorGUILayout.Vector4Field("値(Vector4)", t9.value);
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
            r.width = target.windowX;
            r.height = target.windowY;

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
            else if (rect.x >= target.nodeEditorSize.width - target.windowX)
            {
                rect.position = new Vector2(target.nodeEditorSize.width - target.windowX, rect.y);
            }

            if (rect.y <= 0)
            {
                rect.position = new Vector2(rect.x, 0);
            }
            else if (rect.y >= target.nodeEditorSize.height - target.windowY)
            {
                rect.position = new Vector2(rect.x, target.nodeEditorSize.height - target.windowY);
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
            var list = (ArrayList) obj;
            var pos = (Vector2) list[0];
            var sType = (Type) list[1];

            var cn = (Content) target.gameObject.AddComponent(sType);
            cn.hideFlags = HideFlags.HideInInspector;
            cn.SetRect(new Rect(pos, Vector2.zero));
        }
    }
}
#endif