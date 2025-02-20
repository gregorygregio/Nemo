

namespace Nemo.Tools.ElementTreeNodes
{
    public class ElementTreeNode
    {
        public ElementTreeNode() {}
        public ElementTreeNode? Next { get; set; }
        public bool Rendered { get; set; }

        public virtual string GetElementAction() {
            return string.Empty;
        }
        public virtual object[] GetElementParams(int offsetX, int offsetY) {
            return new object[0];
        }

        public IEnumerable<ElementTreeNode> GetNodes() 
        {
            ElementTreeNode? current = this;
            while(current != null) {
                yield return current;
                current = current.Next;
            }
        }
    }
}