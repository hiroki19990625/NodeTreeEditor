using UnityEngine;

namespace NodeTreeEditor.Variables
{
    public class Vector2Value : Value
    {
        public Vector2 value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(Vector2 v)
        {
            value = v;
        }
    }
}