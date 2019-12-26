using System.Collections;
using NodeTreeEditor.Utils;
using UnityEngine;
#if UNITY_EDITOR
using NodeTreeEditor.Window;
using UnityEditor;

#endif

namespace NodeTreeEditor.Contents
{
    /// <summary>
    /// Random selector.
    /// </summary>
    [AddComponentMenu("NodeTreeEditor/Content/RandomSelector")]
    public class RandomSelector : Content
    {
        [HideInInspector] public RandomTree tree = new RandomTree();

        public override IEnumerator Invoke()
        {
            if (tree.treenodes.Count == 0)
            {
                Debug.LogError("[エラー]条件が設定されていません。");
                yield break;
            }

            var rand = RandomTree.GetIndex(tree);
            if (rand != -1)
            {
                yield return tree.treenodes[rand].next.Invoke();
                yield break;
            }

            if (next == null)
            {
                Debug.LogError("[エラー]その他が設定されていません。");
                yield break;
            }

            yield return next.Invoke();
        }

#if UNITY_EDITOR

        public override string GetDescription()
        {
            return "ランダムで分岐します。";
        }

        public override Color WindowColor()
        {
            return new Color32(0, 120, 85, 255);
        }

        public override void Draw()
        {
            tree.maxRandomNum = EditorGUILayout.IntField("乱数最大値", tree.maxRandomNum);

            GUILayout.BeginVertical(GUI.skin.box);
            {
                RandomTree.TreeNode removeNode = null;
                int diff = tree.maxRandomNum;
                int c = 0;
                EditorGUILayout.Space();
                foreach (var node in tree.treenodes)
                {
                    GUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.LabelField("Node " + (c + 1));
                        node.randomNum = EditorGUILayout.IntSlider("乱数", node.randomNum, 0, diff);
                        diff -= node.randomNum;

                        if (node.next == null)
                        {
                            EditorGUILayout.HelpBox("ノードの接続先が見つかりません。", MessageType.Error);
                        }

                        GUILayout.BeginHorizontal(GUI.skin.box);
                        {
                            if (GUILayout.Button("削除"))
                            {
                                if (EditorUtility.DisplayDialog("Warning", "Node " + (c + 1) + "を削除しますか?", "OK",
                                    "キャンセル"))
                                {
                                    removeNode = node;
                                }
                            }

                            if (node.next != null)
                            {
                                if (GUILayout.Button("リンクを解除"))
                                {
                                    if (EditorUtility.DisplayDialog("Warning", "Node " + (c + 1) + "のリンクを解除しますか?", "OK",
                                        "キャンセル"))
                                    {
                                        node.next = null;
                                    }
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                        c++;
                    }
                    GUILayout.EndVertical();
                }

                if (removeNode != null)
                {
                    tree.treenodes.Remove(removeNode);
                }
            }
            GUILayout.EndVertical();
            if (GUILayout.Button("ノードを追加..."))
            {
                tree.treenodes.Add(new RandomTree.TreeNode());
            }
        }

        public override void ButtonDraw(NodeEditorWindow window)
        {
            if (window.flag)
            {
                if (GUILayout.Button("接続"))
                {
                    window.flag = false;
                    Connect(window);
                }
                else if (GUILayout.Button("キャンセル"))
                {
                    window.flag = false;
                    Cancel(window);
                }
            }
            else
            {
                if (GUILayout.Button("削除"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "このコンテンツを削除しますか?", "OK", "キャンセル"))
                    {
                        Remove();
                    }
                }
                else if (GUILayout.Button("リンク"))
                {
                    window.flag = true;
                    Link(window);
                }
                else if (GUILayout.Button("全てリンク解除"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "このコンテンツのリンクを解除しますか?", "OK", "キャンセル"))
                    {
                        UnLink();
                    }
                }
                else if (GUILayout.Button("その他のリンク解除"))
                {
                    if (EditorUtility.DisplayDialog("Warning", "このコンテンツのリンクを解除しますか?", "OK", "キャンセル"))
                    {
                        next = null;
                    }
                }
            }
        }

        public override void UnLink()
        {
            foreach (RandomTree.TreeNode node in tree.treenodes)
            {
                node.next = null;
            }

            next = null;
        }

        protected override void ConnectClient(Content content)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Other"), false, GDOther, content);
            int c = 0;

            foreach (RandomTree.TreeNode node in tree.treenodes)
            {
                menu.AddItem(new GUIContent("Node " + (c + 1)), false, GDConnect, new ArrayList()
                {
                    c,
                    content,
                    node
                });
                c++;
            }

            menu.ShowAsContext();
        }

        void GDOther(object obj)
        {
            next = (Content) obj;
        }

        void GDConnect(object obj)
        {
            var al = (ArrayList) obj;
            var index = (int) al[0];
            var content = (Content) al[1];

            tree.treenodes[index].next = content;
        }

        public override Color LineColor()
        {
            return new Color32(0, 120, 85, 255);
        }

        public override void LineDraw()
        {
            int c = 0;
            foreach (RandomTree.TreeNode node in tree.treenodes)
            {
                if (node.next != null)
                {
                    ConnectLine(this, node.next, LineColor(), c + 1);
                }

                c++;
            }

            if (next != null)
            {
                ConnectLine(this, next, Color.black);
            }
        }

        protected virtual void ConnectLine(Content to, Content from, Color color, int index)
        {
            Rect start = to.GetRect();
            Rect end = from.GetRect();
            Vector3 startPos = new Vector3(start.x + (start.width / 2), start.y + (start.height / 2), 0);
            Vector3 endPos = new Vector3(end.x + (end.width / 2), end.y + (end.height / 2), 0);
            Vector3 dif = endPos - startPos;
            float rad = Mathf.Atan2(dif.y, dif.x);

            Quaternion f = Quaternion.Euler(0, 0, rad * (180f / Mathf.PI));

            Vector3 centerP = new Vector3((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2, 0);
            Handles.color = color;
            Handles.DrawLine(startPos, endPos);

            Handles.DrawSolidRectangleWithOutline(
                new Vector3[]
                {
                    (f * (Vector3.down * 10)) + centerP, (f * (Vector3.right * 20)) + centerP,
                    (f * (Vector3.up * 10)) + centerP, (f * (Vector3.down * 10)) + centerP
                }, color, color);
            //Handles.DrawPolyLine((f * (Vector3.down * 10)) + centerP, (f * (Vector3.right * 20)) + centerP, (f * (Vector3.up * 10)) + centerP, (f * (Vector3.down * 10)) + centerP);
            GUI.Label(new Rect(centerP.x, centerP.y, 200, 20), "Node" + index);

            Handles.color = Color.black;
        }
#endif
    }
}