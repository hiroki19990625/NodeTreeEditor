using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeTreeEditor.Contents {
	/// <summary>
	/// Delay.
	/// </summary>
	[AddComponentMenu("NodeTreeEditor/Content/Delay")]
	public class Delay : Content {

		[HideInInspector]
		public bool isRealTime = false;
		[HideInInspector]
		public float time = 0f;

		public override IEnumerator Invoke ()
		{
			if (isRealTime) {
				yield return new WaitForSecondsRealtime (time);
			} else {
				yield return new WaitForSeconds (time);
			}
			yield return next.Invoke ();
		}

		#if UNITY_EDITOR

		public override string GetDescription ()
		{
			return "実行を指定の時間、遅延させます。";
		}

		public override Color WindowColor ()
		{
			return new Color32 (255, 120, 255, 255);
		}

		public override void Draw ()
		{
			isRealTime = EditorGUILayout.Toggle ("リアルタイム", isRealTime);
			time = EditorGUILayout.FloatField ("時間(s)", time);
		}

		public override Color LineColor ()
		{
			return new Color32 (255, 120, 255, 255);
		}
		#endif
	}
}