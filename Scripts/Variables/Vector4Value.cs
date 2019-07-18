using UnityEngine;

namespace NodeTreeEditor.Variables
{
    public class Vector4Value : Value
    {
        public Vector4 value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(Vector4 v)
        {
            value = v;
        }
    }
}