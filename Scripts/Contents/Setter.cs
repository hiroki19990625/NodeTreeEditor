using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NodeTreeEditor.Utils;
using NodeTreeEditor.Variables;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Setter.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/TODO/Settor")]
    public class Setter : Content
    {
        [HideInInspector] public List<SetterMethodInvoker> list = new List<SetterMethodInvoker>();

        public override IEnumerator Invoke()
        {
            foreach (var l in list)
            {
                yield return StartCoroutine(l.Invoke(this));
            }

            yield return next.Invoke();
        }

#if UNITY_EDITOR

        public override void Draw()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            {
                SetterMethodInvoker rem = null;
                foreach (var i in list)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        i.nonObject = EditorGUILayout.Toggle("オブジェクトではない", i.nonObject);
                        if (i.nonObject)
                        {
                            EditorGUILayout.Space();
                            i.classTargetName = EditorGUILayout.TextField("ターゲットクラス", i.classTargetName);

                            i.target = (SetterMethodInvoker.Target) EditorGUILayout.EnumPopup("テンプレート", i.target);
                            if (i.target == SetterMethodInvoker.Target.System)
                            {
                                i.classTargetName = "System.Object";
                            }
                            else if (i.target == SetterMethodInvoker.Target.UnityEngine)
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


                                        var mt = i.GetSelectedMethod();
                                        if (mt == null)
                                        {
                                        }
                                        else
                                        {
                                            i.useLocal = EditorGUILayout.Toggle("Use Local", i.useLocal);
                                            if (i.useLocal)
                                            {
                                                i.variableTarget = transform.Find("LocalVariable").gameObject;
                                            }
                                            else
                                            {
                                                i.variableTarget = (GameObject) EditorGUILayout.ObjectField("戻り値",
                                                    i.variableTarget,
                                                    typeof(GameObject), true);
                                            }

                                            if (i.variableTarget != null && GUILayout.Button("変数を選択"))
                                            {
                                                Value[] values = FindVariable(i.variableTarget);
                                                GenericMenu menu = new GenericMenu();
                                                Type type = GetReturnVariableType(mt.ReturnType);
                                                foreach (Value value in values)
                                                {
                                                    if (value.GetType().Name == type.Name)
                                                        menu.AddItem(
                                                            new GUIContent(
                                                                value.valueName + " " +
                                                                (value.constValue ? "(const)" : "")),
                                                            false,
                                                            data =>
                                                            {
                                                                Value select = (Value) data;
                                                                i.returnVariable = select;
                                                            }, value);
                                                }

                                                menu.ShowAsContext();
                                            }
                                            else if (i.returnVariable == null)
                                            {
                                                EditorGUILayout.HelpBox("オブジェクトが見つかりません。", MessageType.Error);
                                            }

                                            GUILayout.BeginVertical(GUI.skin.box);
                                            {
                                                foreach (var pmt in i.parameters)
                                                {
                                                    pmt.ShowField();
                                                }
                                            }
                                            GUILayout.EndVertical();
                                        }
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
                                                o => { i.invokeObject = (Component) o; }, c);
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


                                    var mt = i.GetSelectedMethod();
                                    if (mt == null)
                                    {
                                    }
                                    else
                                    {
                                        Type type = GetReturnVariableType(mt.ReturnType);
                                        EditorGUILayout.HelpBox("変数の型: " + type.Name,
                                            MessageType.Info);

                                        try
                                        {
                                            i.useLocal = EditorGUILayout.Toggle("Use Local", i.useLocal);
                                            if (i.useLocal)
                                            {
                                                i.variableTarget = transform.Find("LocalVariable").gameObject;
                                            }
                                            else
                                            {
                                                i.variableTarget = (GameObject) EditorGUILayout.ObjectField("戻り値",
                                                    i.variableTarget,
                                                    typeof(GameObject), true);
                                            }
                                        }
                                        catch (MissingReferenceException _)
                                        {
                                            return;
                                        }

                                        if (i.variableTarget != null && GUILayout.Button("変数を選択"))
                                        {
                                            Value[] values = FindVariable(i.variableTarget);
                                            GenericMenu menu = new GenericMenu();
                                            foreach (Value value in values)
                                            {
                                                if (value.GetType().Name == type.Name)
                                                    menu.AddItem(
                                                        new GUIContent(
                                                            value.valueName + " " +
                                                            (value.constValue ? "(const)" : "")),
                                                        false,
                                                        data =>
                                                        {
                                                            Value select = (Value) data;
                                                            i.returnVariable = select;
                                                        }, value);
                                            }

                                            menu.ShowAsContext();
                                        }
                                        else if (i.variableTarget == null)
                                        {
                                            EditorGUILayout.HelpBox("オブジェクトが見つかりません。", MessageType.Error);
                                        }

                                        if (i.returnVariable != null)
                                        {
                                            EditorGUILayout.HelpBox("変数: " + i.returnVariable.valueName,
                                                MessageType.Info);
                                        }
                                        else
                                        {
                                            EditorGUILayout.HelpBox("変数が設定されていません。", MessageType.Error);
                                        }

                                        foreach (var pmt in i.parameters)
                                        {
                                            GUILayout.BeginVertical(GUI.skin.box);
                                            {
                                                if (pmt._content == null)
                                                    pmt._content = this;
                                                pmt.ShowField();
                                            }
                                            GUILayout.EndVertical();
                                        }
                                    }
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
                list.Add(new SetterMethodInvoker());
            }
        }

        void ClassSelect(object obj)
        {
            var l = (ArrayList) obj;
            var i = (SetterMethodInvoker) l[0];
            var s = (string) l[1];
            i.classType = s;
        }

        void MethodSelect(object obj)
        {
            var l = (ArrayList) obj;
            var i = (SetterMethodInvoker) l[0];
            var s = (string) l[1];
            var p = (ParameterInfo[]) l[2];
            i.ClearParameter();
            i.selectedMethod = s;
            foreach (var pp in p)
            {
                var ppp = new SetterMethodInvoker.Parameter(this);
                ppp.SetParameterType(pp.ParameterType);
                ppp.parameterName = pp.Name;
                i.parameters.Add(ppp);
            }
        }

        Value[] FindVariable(GameObject obj)
        {
            return obj.GetComponents<Value>().Where(v => !v.constValue).ToArray();
        }

        Type GetReturnVariableType(Type type)
        {
            switch (type.Name)
            {
                case "Single":
                    return typeof(FloatValue);

                case "Boolean":
                    return typeof(BoolValue);

                case "String":
                    return typeof(StringValue);

                case "Int32":
                    return typeof(IntValue);

                case "Vector2":
                    return typeof(Vector2Value);

                case "Vector3":
                    return typeof(Vector3Value);

                case "Vector4":
                    return typeof(Vector4Value);

                case "Object":
                    return typeof(ObjectValue);

                case "GameObject":
                    return typeof(GameObjectValue);
            }

            return typeof(void);
        }

        public override string GetDescription()
        {
            return "代入<TODO>";
        }

        public override Color WindowColor()
        {
            return new Color32(60, 130, 130, 255);
        }

        public override Color LineColor()
        {
            return new Color32(60, 130, 130, 255);
        }
#endif
    }
}