using UnityEngine;
using System.Collections.Generic;

namespace NodeTreeEditor.Variables {
	/// <summary>
	/// Value.
	/// </summary>
	public abstract class Value : MonoBehaviour {

		public enum ValueType {
			Int,
			Float,
			Bool,
			String
		}

		public string valueName = "Variable";

		public bool constValue;

		//[Attributes.Disable]
		[HideInInspector]
		public ValueType valueType;

		public abstract object GetValue ();

		public static string[] VariablesToString (List<Value> variables) {
			var s = new List<string> ();
			foreach (var v in variables) {
				object obj = null;
				if (v == null) {
					continue;
				}
				obj = v.GetValue ();
				s.Add ("<" + v.ToString () + ">" + v.valueName + " => " + obj.ToString () + "");
			}
			return s.ToArray ();
		}
	}
}