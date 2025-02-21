using Nemo.Tools.ElementTreeNodes;

namespace Nemo.Memento
{
    public record CanvasSnapshot
    {
        public FrameNode RootFrameNode { get; set; }
        public CanvasSnapshot(FrameNode frame)
        {
            RootFrameNode = new FrameNode(frame);
            ElementTreeNode current = RootFrameNode;
            foreach(var node in frame.GetNodes())
            {
                if(typeof(FrameNode).IsInstanceOfType(node))
                {
                    continue;
                }
                current.Next = node.Clone();
                current = current.Next;
            }
        }
    }
}