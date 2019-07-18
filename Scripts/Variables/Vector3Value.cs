using UnityEngine;

namespace NodeTreeEditor.Variables
{
    public class Vector3Value : Value
    {
        public Vector3 value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(Vector3 v)
        {
            value = v;
        }
    }
}