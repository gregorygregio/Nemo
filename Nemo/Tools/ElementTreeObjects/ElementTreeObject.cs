

namespace Nemo.Tools.ElementTreeObjects
{
    public class ElementTreeObject
    {
        public ElementTreeObject() {}
        public ElementTreeObject(bool isRoot)
        {
            IsRoot = isRoot;
        }
        public ElementTreeObject? Next { get; set; }
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