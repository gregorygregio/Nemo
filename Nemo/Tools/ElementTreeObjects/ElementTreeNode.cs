

namespace Nemo.Tools.ElementTreeObjects
{
    public class ElementTreeNode
    {
        public ElementTreeNode() {}
        public ElementTreeNode(bool isRoot)
        {
            IsRoot = isRoot;
        }
        public ElementTreeNode? Next { get; set; }
        public bool IsRoot { get; set; } = false;
        public bool Rendered { get; set; }

        public virtual string GetElementAction() {
            return string.Empty;
        }
        public virtual object[] GetElementParams() {
            return new object[0];
        }
    }
}