using UnityEngine;
using System.Collections;

using NodeTreeEditor.Contents;
using NodeTreeEditor.Variables;

namespace NodeTreeEditor.Utils {
	/// <summary>
	/// Conditions.
	/// </summary>
	[System.Serializable]
	public class Conditions {

		public enum CondType {
			Equal,
			NotEqual,
			Big_Small,
			Small_Big,
			Big_Small_Equal,
			Small_Big_Equal
		}

		public enum ValueType {
			Raw,
			Variable,
		}

		public Content next;

		public CondType type;

		public ValueType valueTypeA;
		public Value.ValueType SysTypeA;

		public Value valueA;
		public int rawInt;
		public float rawFloat;
		public bool rawBool;
		public string rawString;

		public Value valueB;

		public bool DoConditions () {
			switch (type) {

			case CondType.Equal:
				if (IsVariable ()) {
					if (EqualVariableType ()) {
						if (valueA.valueType == Value.ValueType.Int) {
							var t1 = (IntValue)valueA;
							var t2 = (IntValue)valueB;

							if (t1.value == t2.value) {	
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Float) {
							var t1 = (FloatValue)valueA;
							var t2 = (FloatValue)valueB;

							if (t1.value == t2.value) {
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Bool) {
							var t1 = (BoolValue)valueA;
							var t2 = (BoolValue)valueB;

							if (t1.value == t2.value) {
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.String) {
							var t1 = (StringValue)valueA;
							var t2 = (StringValue)valueB;

							if (t1.value == t2.value) {
								return true;
							}
						}
					}
				} else {
					if (SysTypeA == Value.ValueType.Int) {
						var t2 = (IntValue)valueB;

						if (rawInt == t2.value) {	
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Float) {
						var t2 = (FloatValue)valueB;

						if (rawFloat == t2.value) {
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Bool) {
						var t2 = (BoolValue)valueB;

						if (rawBool == t2.value) {
							return true;
						}
					} else if (SysTypeA == Value.ValueType.String) {
						var t2 = (StringValue)valueB;

						if (rawString == t2.value) {
							return true;
						}
					}
				}
				break;

			case CondType.NotEqual:
				if (IsVariable ()) {
					if (EqualVariableType ()) {
						if (valueA.valueType == Value.ValueType.Int) {
							var t1 = (IntValue)valueA;
							var t2 = (IntValue)valueB;

							if (t1.value != t2.value) {	
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Float) {
							var t1 = (FloatValue)valueA;
							var t2 = (FloatValue)valueB;

							if (t1.value != t2.value) {
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Bool) {
							var t1 = (BoolValue)valueA;
							var t2 = (BoolValue)valueB;

							if (t1.value != t2.value) {
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.String) {
							var t1 = (StringValue)valueA;
							var t2 = (StringValue)valueB;

							if (t1.value != t2.value) {
								return true;
							}
						}
					}
				} else {
					if (SysTypeA == Value.ValueType.Int) {
						var t2 = (IntValue)valueB;

						if (rawInt != t2.value) {	
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Float) {
						var t2 = (FloatValue)valueB;

						if (rawFloat != t2.value) {
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Bool) {
						var t2 = (BoolValue)valueB;

						if (rawBool != t2.value) {
							return true;
						}
					} else if (SysTypeA == Value.ValueType.String) {
						var t2 = (StringValue)valueB;

						if (rawString != t2.value) {
							return true;
						}
					}
				}
				break;

			case CondType.Big_Small:
				if (IsVariable ()) {
					if (EqualVariableType ()) {
						if (valueA.valueType == Value.ValueType.Int) {
							var t1 = (IntValue)valueA;
							var t2 = (IntValue)valueB;

							if (t1.value > t2.value) {	
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Float) {
							var t1 = (FloatValue)valueA;
							var t2 = (FloatValue)valueB;

							if (t1.value > t2.value) {
								return true;
							}
						}
					}
				} else {
					if (SysTypeA == Value.ValueType.Int) {
						var t2 = (IntValue)valueB;

						if (rawInt > t2.value) {	
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Float) {
						var t2 = (FloatValue)valueB;

						if (rawFloat > t2.value) {
							return true;
						}
					}
				}
				break;

			case CondType.Big_Small_Equal:
				if (IsVariable ()) {
					if (EqualVariableType ()) {
						if (valueA.valueType == Value.ValueType.Int) {
							var t1 = (IntValue)valueA;
							var t2 = (IntValue)valueB;

							if (t1.value >= t2.value) {	
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Float) {
							var t1 = (FloatValue)valueA;
							var t2 = (FloatValue)valueB;

							if (t1.value >= t2.value) {
								return true;
							}
						}
					}
				} else {
					if (SysTypeA == Value.ValueType.Int) {
						var t2 = (IntValue)valueB;

						if (rawInt >= t2.value) {	
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Float) {
						var t2 = (FloatValue)valueB;

						if (rawFloat >= t2.value) {
							return true;
						}
					}
				}
				break;

			case CondType.Small_Big:
				if (IsVariable ()) {
					if (EqualVariableType ()) {
						if (valueA.valueType == Value.ValueType.Int) {
							var t1 = (IntValue)valueA;
							var t2 = (IntValue)valueB;

							if (t1.value < t2.value) {	
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Float) {
							var t1 = (FloatValue)valueA;
							var t2 = (FloatValue)valueB;

							if (t1.value < t2.value) {
								return true;
							}
						}
					}
				} else {
					if (SysTypeA == Value.ValueType.Int) {
						var t2 = (IntValue)valueB;

						if (rawInt < t2.value) {	
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Float) {
						var t2 = (FloatValue)valueB;

						if (rawFloat < t2.value) {
							return true;
						}
					}
				}
				break;

			case CondType.Small_Big_Equal:
				if (IsVariable ()) {
					if (EqualVariableType ()) {
						if (valueA.valueType == Value.ValueType.Int) {
							var t1 = (IntValue)valueA;
							var t2 = (IntValue)valueB;

							if (t1.value <= t2.value) {	
								return true;
							}
						} else if (valueA.valueType == Value.ValueType.Float) {
							var t1 = (FloatValue)valueA;
							var t2 = (FloatValue)valueB;

							if (t1.value <= t2.value) {
								return true;
							}
						}
					}
				} else {
					if (SysTypeA == Value.ValueType.Int) {
						var t2 = (IntValue)valueB;

						if (rawInt <= t2.value) {	
							return true;
						}
					} else if (SysTypeA == Value.ValueType.Float) {
						var t2 = (FloatValue)valueB;

						if (rawFloat <= t2.value) {
							return true;
						}
					}
				}
				break;
			}
			return false;
		}

		public bool IsVariable () {
			if (valueTypeA == ValueType.Variable) {
				return true;
			}
			return false;
		}

		public bool EqualVariableType () {
			if (valueA.valueType == valueB.valueType) {
				return true;
			}
			return false;
		}
	}
}