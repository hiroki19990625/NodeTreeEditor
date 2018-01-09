using UnityEngine;

namespace NodeTreeEditor.Variables
{
    /// <summary>
    /// Bool value.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Values/BoolValue")]
    class BoolValue : Value
    {

        public bool value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(bool v)
        {
            value = v;
        }

        public void SetReverseValue()
        {
            if (value)
            {
                value = false;
            }
            else
            {
                value = true;
            }
        }
    }
}