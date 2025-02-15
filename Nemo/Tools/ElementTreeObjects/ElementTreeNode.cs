

namespace Nemo.Tools.ElementTreeNodes
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
        protected int offsetX = 0;
        protected int offsetY = 0;
        public void ApplyOffset(int offsetX, int offsetY) {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }
    }
}