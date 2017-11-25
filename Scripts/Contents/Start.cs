using UnityEngine;
using System.Collections;

using NodeTreeEditor.Window;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeTreeEditor.Contents {
	/// <summary>
	/// Start.
	/// </summary>
	[AddComponentMenu("NodeTreeEditor/Content/Base/Start")]
	public class Start : Content {

		public Start () {
			commonName = "Start";
		}

		public override IEnumerator Invoke ()
		{
			if (next == null) yield break;
			yield return next.Invoke ();
		}

		#if UNITY_EDITOR

		public override string GetDescription ()
		{
			return "エントリーポイントです。(開始地点)";
		}

		public override Color WindowColor ()
		{
			return new Color32 (255, 255, 255, 255);
		}

		public override void ButtonDraw (NodeEditorWindow window)
		{
			if (window.flag) {
				if (GUILayout.Button ("接続")) {
					window.flag = false;
					Connect (window);
				} else if (GUILayout.Button ("キャンセル")){
					window.flag = false;
					Cancel (window);
				}
			} else {
				if (GUILayout.Button ("リンク")) {
					window.flag = true;
					Link (window);
				} else if (GUILayout.Button ("リンク解除")) {
					if (EditorUtility.DisplayDialog ("Warning", "このコンテンツのリンクを解除しますか?", "OK", "キャンセル")) {
						UnLink ();
					}
				}
			}
		}
		#endif
	}
}

