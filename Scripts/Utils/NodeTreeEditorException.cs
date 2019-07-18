using System;

namespace NodeTreeEditor.Utils
{
    /// <summary>
    /// Node tree editor exception.
    /// </summary>
    public class NodeTreeEditorException : Exception
    {
        public NodeTreeEditorException(string message) : base(message)
        {
        }

        public NodeTreeEditorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}