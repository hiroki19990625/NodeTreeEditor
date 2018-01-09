using UnityEngine;

namespace NodeTreeEditor.Variables
{
    /// <summary>
    /// Float value.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Values/FloatValue")]
    class FloatValue : Value
    {

        public float value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(float v)
        {
            value = v;
        }

        public void AddValue(float v)
        {
            value = v;
        }

        public void SetReverseValue()
        {
            if (value == 0f)
            {
            }
            else if (value > 0f)
            {
                value = -value;
            }
            else
            {
                value = Mathf.Sign(value);
            }
        }
    }
}