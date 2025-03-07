

namespace Nemo.Tools.ElementTreeNodes
{
    public abstract class ElementTreeNode
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

        public abstract ElementTreeNode Clone();

        public IEnumerable<ElementTreeNode> GetNodes() 
        {
            ElementTreeNode? current = this;
            while(current != null) {
                yield return current;
                current = current.Next;
            }
        }

        public override string ToString()
        {
            return string.Format("\n  {0}({1}){2}", GetElementAction(), GetElementParams(0, 0).Select(x => x.ToString()).Aggregate((a, b) => a + ", " + b), Next?.ToString());
        }
    }
}