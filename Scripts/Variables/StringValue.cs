using UnityEngine;

namespace NodeTreeEditor.Variables
{
    /// <summary>
    /// String value.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Values/StringValue")]
    class StringValue : Value
    {
        public string value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(string v)
        {
            value = v;
        }
    }
}