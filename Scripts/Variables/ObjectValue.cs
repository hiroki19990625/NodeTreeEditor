using UnityEngine;

namespace NodeTreeEditor.Variables
{
    public class ObjectValue : Value
    {
        public Object value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(Object v)
        {
            value = v;
        }
    }
}