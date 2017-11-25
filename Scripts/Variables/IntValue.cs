using UnityEngine;

namespace NodeTreeEditor.Variables {
	/// <summary>
	/// Int value.
	/// </summary>
	[AddComponentMenu("NodeTreeEditor/Values/IntValue")]
	class IntValue : Value {

		public int value;

		public override object GetValue (){
			return value;
		}

		public void SetValue (int v) {
			value = v;
		}

		public void AddValue (int v) {
			value = v;
		}

		public void SetReverseValue () {
			if (value == 0) {
			} else if (value > 0) {
				value = -value;
			} else {
				value = Mathf.Abs (value);
			}
		}
	}
}