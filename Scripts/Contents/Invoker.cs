using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeTreeEditor.Utils;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Invoker.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/Invoker")]
    public class Invoker : Content
    {
        [HideInInspector] public List<MethodInvoker> list = new List<MethodInvoker>();

        public override IEnumerator Invoke()
        {
            foreach (var l in list)
            {
                yield return StartCoroutine(l.Invoke(this));
            }

            yield return next.Invoke();
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "メソッド実行(<注意>実行速度が遅い)";
        }

        public override Color WindowColor()
        {
            return new Color32(255, 150, 0, 255);
        }

        public override void Draw()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                MethodInvoker rem = null;
                foreach (var i in list)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        i.nonObject = EditorGUILayout.Toggle("オブジェクトではない", i.nonObject);
                        if (i.nonObject)
                        {
                            EditorGUILayout.Space();
                            i.classTargetName = EditorGUILayout.TextField("ターゲットクラス", i.classTargetName);

                            i.target = (MethodInvoker.Target) EditorGUILayout.EnumPopup("テンプレート", i.target);
                            if (i.target == MethodInvoker.Target.System)
                            {
                                i.classTargetName = "System.Object";
                            }
                            else if (i.target == MethodInvoker.Target.UnityEngine)
                            {
                                i.classTargetName = "UnityEngine.Object, UnityEngine";
                            }

                            i.classSerach = EditorGUILayout.TextField("検索", i.classSerach);

                            if (i.classTargetName != "")
                            {
                                if (GUILayout.Button("クラスを選択"))
                                {
                                    GenericMenu menu = new GenericMenu();
                                    foreach (var s in i.GetAllClass(i.classSerach))
                                    {
                                        menu.AddItem(new GUIContent(s), false, ClassSelect, new ArrayList()
                                        {
                                            i,
                                            s
                                        });
                                    }

                                    menu.ShowAsContext();
                                }

                                if (i.classType != "")
                                {
                                    GUILayout.Label("選択済:  " + i.classType);
                                    if (GUILayout.Button("メソッドを選択"))
                                    {
                                        GenericMenu menu = new GenericMenu();
                                        var names = i.GetTypeMethodNames();
                                        var parameter = i.GetTypeMethodParameters();
                                        var ind = 0;
                                        foreach (var s in i.GetTypeMethodLabel())
                                        {
                                            menu.AddItem(new GUIContent(s), false, MethodSelect, new ArrayList()
                                            {
                                                i,
                                                names[ind],
                                                parameter[ind]
                                            });
                                            ind++;
                                        }

                                        menu.ShowAsContext();
                                    }

                                    if (i.selectedMethod != "")
                                    {
                                        GUILayout.Label("選択済:  " + i.selectedMethod);

                                        GUILayout.BeginVertical(GUI.skin.box);
                                        {
                                            var mt = i.GetSelectedMethod();
                                            if (mt == null)
                                            {
                                            }
                                            else
                                            {
                                                foreach (var pmt in i.parameters)
                                                {
                                                    pmt.ShowField();
                                                }
                                            }
                                        }
                                        GUILayout.EndVertical();
                                    }
                                }
                            }
                        }
                        else
                        {
                            i.invokeObject = EditorGUILayout.ObjectField(i.invokeObject, typeof(Object), true);
                            if (i.invokeObject != null)
                            {
                                if (i.invokeObject is GameObject)
                                {
                                    if (GUILayout.Button("Componentを選択"))
                                    {
                                        GenericMenu menu = new GenericMenu();
                                        Component[] comps = ((GameObject) i.invokeObject).GetComponents<Component>();
                                        foreach (Component c in comps)
                                        {
                                            menu.AddItem(
                                                new GUIContent(c.GetType().Name + "<" + c.GetInstanceID() + ">"), false,
                                                (object o) => { i.invokeObject = (Component) o; }, c);
                                        }

                                        menu.ShowAsContext();
                                    }
                                }

                                EditorGUILayout.Space();
                                if (GUILayout.Button("メソッドを選択"))
                                {
                                    GenericMenu menu = new GenericMenu();
                                    var names = i.GetInvokeObjectMethodNames();
                                    var parameter = i.GetInvokeObjectMethodParameters();
                                    var ind = 0;
                                    foreach (var s in i.GetInvokeObjectMethodLabel())
                                    {
                                        menu.AddItem(new GUIContent(s), false, MethodSelect, new ArrayList()
                                        {
                                            i,
                                            names[ind],
                                            parameter[ind]
                                        });
                                        ind++;
                                    }

                                    menu.ShowAsContext();
                                }

                                if (i.selectedMethod != "")
                                {
                                    GUILayout.Label("選択済:  " + i.selectedMethod);

                                    GUILayout.BeginVertical(GUI.skin.box);
                                    {
                                        var mt = i.GetSelectedMethod();
                                        if (mt == null)
                                        {
                                        }
                                        else
                                        {
                                            foreach (var pmt in i.parameters)
                                            {
                                                pmt.ShowField();
                                            }
                                        }
                                    }
                                    GUILayout.EndVertical();
                                }
                            }
                            else
                            {
                                i.selectedMethod = "";
                            }
                        }

                        if (GUILayout.Button("削除"))
                        {
                            if (EditorUtility.DisplayDialog("Warning", "このコンテンツを削除しますか?", "OK", "キャンセル"))
                            {
                                rem = i;
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }

                if (rem != null)
                {
                    list.Remove(rem);
                }
            }
            GUILayout.EndVertical();

            if (GUILayout.Button("追加"))
            {
                list.Add(new MethodInvoker());
            }
        }

        void ClassSelect(object obj)
        {
            var l = (ArrayList) obj;
            var i = (MethodInvoker) l[0];
            var s = (string) l[1];
            i.classType = s;
        }

        void MethodSelect(object obj)
        {
            var l = (ArrayList) obj;
            var i = (MethodInvoker) l[0];
            var s = (string) l[1];
            var p = (System.Reflection.ParameterInfo[]) l[2];
            i.ClearParameter();
            i.selectedMethod = s;
            foreach (var pp in p)
            {
                var ppp = new MethodInvoker.Parameter();
                ppp.SetParameterType(pp.ParameterType);
                ppp.parameterName = pp.Name;
                i.parameters.Add(ppp);
            }
        }

        public override Color LineColor()
        {
            return new Color32(255, 150, 0, 255);
        }
#endif
    }
}