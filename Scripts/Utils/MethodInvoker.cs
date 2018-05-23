using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using NodeTreeEditor.Variables;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeTreeEditor.Utils
{
    /// <summary>
    /// Method invoker.
    /// </summary>
    [System.Serializable]
    public class MethodInvoker
    {

        [System.Serializable]
        public class Parameter
        {

            public enum ParameterType
            {
                Int,
                Float,
                Double,
                Long,
                Byte,
                Short,
                Bool,
                String,
                Vector2,
                Vector3,
                Transform,
                Object,
                MonoBehaviour
            }

            public ParameterType parameterType;

            public string parameterName;

            //Field
            public int v_int;
            public float v_float;
            public double v_double;
            public long v_long;
            public byte v_byte;
            public short v_short;
            public bool v_bool;
            public string v_string = "";
            public Vector2 v_vector2;
            public Vector3 v_vector3;
            public Transform v_transform;
            public Object v_object;
            public MonoBehaviour v_MonoBehaviour;

            public bool useVariable = false;
            public Value variable;

            public void SetParameterType(System.Type type)
            {
                switch (type.Name)
                {

                    case "Single":
                        parameterType = ParameterType.Float;
                        break;

                    case "Double":
                        parameterType = ParameterType.Double;
                        break;

                    case "Boolean":
                        parameterType = ParameterType.Bool;
                        break;

                    case "String":
                        parameterType = ParameterType.String;
                        break;

                    case "Byte":
                        parameterType = ParameterType.Byte;
                        break;

                    case "Int16":
                        parameterType = ParameterType.Short;
                        break;

                    case "Int32":
                        parameterType = ParameterType.Int;
                        break;

                    case "Int64":
                        parameterType = ParameterType.Long;
                        break;

                    case "Vector2":
                        parameterType = ParameterType.Vector2;
                        break;

                    case "Vector3":
                        parameterType = ParameterType.Vector3;
                        break;

                    case "Transform":
                        parameterType = ParameterType.Transform;
                        break;

                    case "Object":
                        parameterType = ParameterType.Object;
                        break;

                    case "MonoBehaviour":
                        parameterType = ParameterType.MonoBehaviour;
                        break;
                }
            }

            public object GetValue()
            {
                object value = null;
                switch (parameterType)
                {

                    case ParameterType.Bool:
                        value = v_bool;
                        break;

                    case ParameterType.Byte:
                        value = v_byte;
                        break;

                    case ParameterType.Double:
                        value = v_double;
                        break;

                    case ParameterType.Float:
                        value = v_float;
                        break;

                    case ParameterType.Int:
                        value = v_int;
                        break;

                    case ParameterType.Long:
                        value = v_long;
                        break;

                    case ParameterType.MonoBehaviour:
                        value = v_MonoBehaviour;
                        break;

                    case ParameterType.Object:
                        value = v_object;
                        break;

                    case ParameterType.Short:
                        value = v_short;
                        break;

                    case ParameterType.String:
                        value = v_string;
                        break;

                    case ParameterType.Transform:
                        value = v_transform;
                        break;

                    case ParameterType.Vector2:
                        value = v_vector2;
                        break;

                    case ParameterType.Vector3:
                        value = v_vector3;
                        break;
                }
                return value;
            }

            public System.Type GetParameterType()
            {
                System.Type type = null;
                switch (parameterType)
                {

                    case ParameterType.Bool:
                        type = typeof(bool);
                        break;

                    case ParameterType.Byte:
                        type = typeof(byte);
                        break;

                    case ParameterType.Double:
                        type = typeof(double);
                        break;

                    case ParameterType.Float:
                        type = typeof(float);
                        break;

                    case ParameterType.Int:
                        type = typeof(int);
                        break;

                    case ParameterType.Long:
                        type = typeof(long);
                        break;

                    case ParameterType.MonoBehaviour:
                        type = typeof(MonoBehaviour);
                        break;

                    case ParameterType.Object:
                        type = typeof(Object);
                        break;

                    case ParameterType.Short:
                        type = typeof(short);
                        break;

                    case ParameterType.String:
                        type = typeof(string);
                        break;

                    case ParameterType.Transform:
                        type = typeof(Transform);
                        break;

                    case ParameterType.Vector2:
                        type = typeof(Vector2);
                        break;

                    case ParameterType.Vector3:
                        type = typeof(Vector3);
                        break;
                }
                return type;
            }

#if UNITY_EDITOR

            public void ShowField()
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                switch (parameterType)
                {

                    case ParameterType.Bool:
                        useVariable = EditorGUILayout.Toggle("Use Variable", useVariable);
                        if (useVariable)
                        {
                            variable = (Value) EditorGUILayout.ObjectField(parameterName, variable, typeof(Value), true);
                            if (variable is BoolValue)
                            {
                                var conv = (BoolValue) variable;
                                v_bool = (bool) conv.GetValue();
                                EditorGUILayout.HelpBox(v_bool + " << 変数の値(" + variable.valueName + ")", MessageType.Info);
                            }
                            else if (variable == null)
                            {
                                EditorGUILayout.HelpBox("変数が設定されていません。", MessageType.Error);
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("サポート外の変数です。<" + variable.valueName + ">", MessageType.Error);
                            }
                        }
                        else
                        {
                            v_bool = EditorGUILayout.Toggle(parameterName, v_bool);
                        }
                        break;

                    case ParameterType.Byte:
                        //v_byte = EditorGUILayout;
                        break;

                    case ParameterType.Double:
                        v_double = EditorGUILayout.DoubleField(parameterName, v_double);
                        break;

                    case ParameterType.Float:
                        useVariable = EditorGUILayout.Toggle("Use Variable", useVariable);
                        if (useVariable)
                        {
                            variable = (Value) EditorGUILayout.ObjectField(parameterName, variable, typeof(Value), true);
                            if (variable is FloatValue)
                            {
                                var conv = (FloatValue) variable;
                                v_float = (float) conv.GetValue();
                                EditorGUILayout.HelpBox(v_float + " << 変数の値(" + variable.valueName + ")", MessageType.Info);
                            }
                            else if (variable == null)
                            {
                                EditorGUILayout.HelpBox("変数が設定されていません。", MessageType.Error);
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("サポート外の変数です。<" + variable.valueName + ">", MessageType.Error);
                            }
                        }
                        else
                        {
                            v_float = EditorGUILayout.FloatField(parameterName, v_float);
                        }
                        break;

                    case ParameterType.Int:
                        useVariable = EditorGUILayout.Toggle("Use Variable", useVariable);
                        if (useVariable)
                        {
                            variable = (Value) EditorGUILayout.ObjectField(parameterName, variable, typeof(Value), true);
                            if (variable is IntValue)
                            {
                                var conv = (IntValue) variable;
                                v_int = (int) conv.GetValue();
                                EditorGUILayout.HelpBox(v_int + " << 変数の値(" + variable.valueName + ")", MessageType.Info);
                            }
                            else if (variable == null)
                            {
                                EditorGUILayout.HelpBox("変数が設定されていません。", MessageType.Error);
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("サポート外の変数です。<" + variable.valueName + ">", MessageType.Error);
                            }
                        }
                        else
                        {
                            v_int = EditorGUILayout.IntField(parameterName, v_int);
                        }
                        break;

                    case ParameterType.Long:
                        v_long = EditorGUILayout.LongField(parameterName, v_long);
                        break;

                    case ParameterType.MonoBehaviour:
                        v_MonoBehaviour = (MonoBehaviour) EditorGUILayout.ObjectField(parameterName, v_MonoBehaviour, typeof(MonoBehaviour), true);
                        break;

                    case ParameterType.Object:
                        v_object = EditorGUILayout.ObjectField(parameterName, v_object, typeof(Object), true);
                        break;

                    case ParameterType.Short:
                        //v_short = EditorGUILayout;
                        break;

                    case ParameterType.String:
                        useVariable = EditorGUILayout.Toggle("Use Variable", useVariable);
                        if (useVariable)
                        {
                            variable = (Value) EditorGUILayout.ObjectField(parameterName, variable, typeof(Value), true);
                            if (variable is StringValue)
                            {
                                var conv = (StringValue) variable;
                                v_string = (string) conv.GetValue();
                                EditorGUILayout.HelpBox(v_string + " << 変数の値(" + variable.valueName + ")", MessageType.Info);
                            }
                            else if (variable == null)
                            {
                                EditorGUILayout.HelpBox("変数が設定されていません。", MessageType.Error);
                            }
                            else
                            {
                                EditorGUILayout.HelpBox("サポート外の変数です。<" + variable.valueName + ">", MessageType.Error);
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField(parameterName);
                            v_string = EditorGUILayout.TextArea(v_string);
                        }
                        break;

                    case ParameterType.Transform:
                        v_transform = (Transform) EditorGUILayout.ObjectField(parameterName, v_transform, typeof(Transform), true);
                        break;

                    case ParameterType.Vector2:
                        v_vector2 = EditorGUILayout.Vector2Field(parameterName, v_vector2);
                        break;

                    case ParameterType.Vector3:
                        v_vector3 = EditorGUILayout.Vector3Field(parameterName, v_vector3);
                        break;
                }
                EditorGUILayout.EndVertical();
            }

#endif
        }

        public enum Target
        {
            None,
            System,
            UnityEngine
        }

        public Object invokeObject;

        public bool nonObject;
        public Target target;
        public string classTargetName = "";
        public string classType = "";
        public string classSerach = "";

        public string selectedMethod = "";

        public List<Parameter> parameters = new List<Parameter>();

        public IEnumerator Invoke(MonoBehaviour mono)
        {
            var m = GetSelectedMethod();
            if (m != null)
            {
                List<object> objs = new List<object>();
                foreach (var p in parameters)
                {
                    objs.Add(p.GetValue());
                }
                if (nonObject)
                {
                    if (m.ReturnType.Name == "IEnumerator")
                    {
                        yield return mono.StartCoroutine((IEnumerator) m.Invoke(null, objs.ToArray()));
                    }
                    else
                    {
                        yield return m.Invoke(null, objs.ToArray());
                    }
                }
                else
                {
                    if (m.ReturnType.Name == "IEnumerator")
                    {
                        yield return mono.StartCoroutine((IEnumerator) m.Invoke(invokeObject, objs.ToArray()));
                    }
                    else
                    {
                        yield return m.Invoke(invokeObject, objs.ToArray());
                    }
                }
            }
            yield return null;
        }

        public List<string> GetAllClass(string targetStr = "")
        {
            List<string> list = new List<string>();
            Assembly asm = Assembly.GetAssembly(System.Type.GetType(classTargetName));
            foreach (var t in asm.GetTypes())
            {
                list.Add(t.FullName);
            }
            if (targetStr != "")
            {
                list = list.FindAll(new System.Predicate<string>((string obj) =>
                        {
                            var c = obj.IndexOf(targetStr, System.StringComparison.OrdinalIgnoreCase);
                            return c != -1;
                        }));
            }
            list.Sort();
            return list;
        }

        public List<string> GetInvokeObjectMethodNames()
        {
            List<string> list = new List<string>();

            if (invokeObject == null)
            {
                return list;
            }
            foreach (var m in invokeObject.GetType().GetMethods())
            {
                if (m.ReturnType == typeof(void) || m.ReturnType == typeof(IEnumerator))
                {
                    var nsp = false;
                    foreach (var p in m.GetParameters())
                    {
                        if (!SupportParameter(p.ParameterType))
                        {
                            nsp = true;
                        }
                    }
                    if (nsp)
                    {
                        continue;
                    }
                    list.Add(m.Name);
                }
            }
            return list;
        }

        public List<ParameterInfo[]> GetInvokeObjectMethodParameters()
        {
            var list = new List<ParameterInfo[]>();

            if (invokeObject == null)
            {
                return list;
            }

            foreach (var m in invokeObject.GetType().GetMethods())
            {
                if (m.ReturnType == typeof(void) || m.ReturnType == typeof(IEnumerator))
                {
                    var nsp = false;
                    foreach (var p in m.GetParameters())
                    {
                        if (!SupportParameter(p.ParameterType))
                        {
                            nsp = true;
                        }
                    }

                    if (nsp)
                    {
                        continue;
                    }
                    list.Add(m.GetParameters());
                }
            }
            return list;
        }

        public List<string> GetInvokeObjectMethodLabel()
        {
            List<string> list = new List<string>();

            if (invokeObject == null)
            {
                return list;
            }

            foreach (var m in invokeObject.GetType().GetMethods())
            {
                string str = "";
                if (m.ReturnType == typeof(void) || m.ReturnType == typeof(IEnumerator))
                {
                    var nsp = false;
                    str = "<" + m.ReturnType.Name + "> " + m.Name + " (";
                    foreach (var p in m.GetParameters())
                    {
                        str += p.ParameterType.Name + ", ";
                        if (!SupportParameter(p.ParameterType))
                        {
                            nsp = true;
                        }
                    }

                    str = str.Remove(str.Length - 2);
                    if (!str.Contains("("))
                    {
                        str += " (";
                    }

                    str += ")";
                    if (nsp)
                    {
                        continue;
                    }
                    list.Add(str);
                }
            }
            return list;
        }

        public List<string> GetTypeMethodNames()
        {
            List<string> list = new List<string>();

            if (classType == null)
            {
                return list;
            }

            var asm = Assembly.GetAssembly(System.Type.GetType(classTargetName));
            foreach (var m in asm.GetType(classType, true).GetMethods())
            {
                if (m.ReturnType == typeof(void) || m.ReturnType == typeof(IEnumerator))
                {
                    if (m.IsStatic)
                    {
                        var nsp = false;
                        foreach (var p in m.GetParameters())
                        {
                            if (!SupportParameter(p.ParameterType))
                            {
                                nsp = true;
                            }
                        }
                        if (nsp)
                        {
                            continue;
                        }
                        list.Add(m.Name);
                    }
                }
            }
            return list;
        }

        public List<ParameterInfo[]> GetTypeMethodParameters()
        {
            var list = new List<ParameterInfo[]>();

            if (classType == null)
            {
                return list;
            }

            var asm = Assembly.GetAssembly(System.Type.GetType(classTargetName));
            foreach (var m in asm.GetType(classType, true).GetMethods())
            {
                if (m.ReturnType == typeof(void) || m.ReturnType == typeof(IEnumerator))
                {
                    if (m.IsStatic)
                    {
                        var nsp = false;
                        foreach (var p in m.GetParameters())
                        {
                            if (!SupportParameter(p.ParameterType))
                            {
                                nsp = true;
                            }
                        }
                        if (nsp)
                        {
                            continue;
                        }
                        list.Add(m.GetParameters());
                    }
                }
            }
            return list;
        }

        public List<string> GetTypeMethodLabel()
        {
            List<string> list = new List<string>();

            if (classType == null)
            {
                return list;
            }

            var asm = Assembly.GetAssembly(System.Type.GetType(classTargetName));
            foreach (var m in asm.GetType(classType, true).GetMethods())
            {
                string str = "";
                if (m.ReturnType == typeof(void) || m.ReturnType == typeof(IEnumerator))
                {
                    if (m.IsStatic)
                    {
                        var nsp = false;
                        str = "<" + m.ReturnType.Name + "> " + m.Name + " (";
                        foreach (var p in m.GetParameters())
                        {
                            str += p.ParameterType.Name + ", ";
                            if (!SupportParameter(p.ParameterType))
                            {
                                nsp = true;
                            }
                        }
                        str = str.Remove(str.Length - 2);
                        if (!str.Contains("("))
                        {
                            str += " (";
                        }
                        str += ")";
                        if (nsp)
                        {
                            continue;
                        }
                        list.Add(str);
                    }
                }
            }
            return list;
        }

        public MethodInfo GetSelectedMethod()
        {
            MethodInfo info = null;
            if (nonObject)
            {
                try
                {
                    var asm = Assembly.GetAssembly(System.Type.GetType(classTargetName));
                    var types = asm.GetTypes();
                    System.Type type = null;
                    foreach (var t in types)
                    {
                        if (t.FullName == classType)
                        {
                            type = t;
                            break;
                        }
                    }
                    if (type != null)
                    {
                        var method = type.GetMethod(selectedMethod, ParameterTypes());
                        if (method != null)
                        {
                            info = method;
                        }
                        else
                        {
                            selectedMethod = "";
                        }
                    }
                    else
                    {
                        classType = "";
                        selectedMethod = "";
                    }
                }
                catch (AmbiguousMatchException e)
                {
                    Debug.LogError("このメソッドは未対応です!(" + selectedMethod + ")");
                    Debug.Log(e);
                    selectedMethod = "";
                }
            }
            else
            {
                try
                {
                    var method = invokeObject.GetType().GetMethod(selectedMethod, ParameterTypes());
                    if (method != null)
                    {
                        info = method;
                    }
                    else
                    {
                        selectedMethod = "";
                    }
                }
                catch (AmbiguousMatchException e)
                {
                    Debug.LogError("このメソッドは未対応です!(" + selectedMethod + ")");
                    Debug.Log(e);
                    selectedMethod = "";
                }
            }
            return info;
        }

        public System.Type[] ParameterTypes()
        {
            List<System.Type> types = new List<System.Type>();
            foreach (var p in parameters)
            {
                types.Add(p.GetParameterType());
            }
            return types.ToArray();
        }

        public void ClearParameter()
        {
            parameters.Clear();
        }

        public bool SupportParameter(System.Type type)
        {
            switch (type.Name)
            {

                case "Single":
                case "Double":
                case "Boolean":
                case "String":
                case "Byte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Vector2":
                case "Vector3":
                case "Transform":
                case "Object":
                case "MonoBehaviour":
                    return true;
            }
            return false;
        }
    }
}