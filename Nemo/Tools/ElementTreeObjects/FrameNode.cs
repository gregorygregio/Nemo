namespace Nemo.Tools.ElementTreeNodes
{
    public class FrameNode : ElementTreeNode
    {
        public FrameNode() 
        {
            OffsetX = 0;
            OffsetY = 0;
            Width = 0;
            Height = 0;
        }
        public FrameNode(int width, int height)
        {
            OffsetX = 0;
            OffsetY = 0;
            Width = width;
            Height = height;
        }
        public FrameNode(int offsetX, int offsetY, int width, int height)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
            Width = width;
            Height = height;
        }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}