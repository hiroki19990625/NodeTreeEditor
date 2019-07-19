using UnityEngine;

namespace NodeTreeEditor.Variables
{
    public class GameObjectValue : Value
    {
        public GameObject value;

        public override object GetValue()
        {
            return value;
        }

        public void SetValue(GameObject v)
        {
            value = v;
        }
    }
}