using System.Collections.Generic;
using NodeTreeEditor.Contents;
using UnityEngine;

namespace NodeTreeEditor.Utils
{
    /// <summary>
    /// Random tree.
    /// </summary>
    [System.Serializable]
    public class RandomTree
    {
        [System.Serializable]
        public class TreeNode
        {
            public int randomNum;
            public Content next;
        }

        public int maxRandomNum;

        public List<TreeNode> treenodes = new List<TreeNode>();

        public static int GetIndex(RandomTree tree)
        {
            int index = 0;
            int sub = 0;
            int random = Random.Range(1, tree.maxRandomNum);
            foreach (TreeNode node in tree.treenodes)
            {
                sub += node.randomNum;
                if (sub >= random)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}