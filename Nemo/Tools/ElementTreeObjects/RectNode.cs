namespace Nemo.Tools.ElementTreeNodes
{
    public class RectNode: ElementTreeNode
    {
        public RectNode(int x, int y, int width, int height, string color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; }

        public override string GetElementAction() {
            return "drawRect";
        }
        public override object[] GetElementParams(int offsetX, int offsetY) {
            return new object[5] {
                X + offsetX, Y + offsetY, Width, Height, Color
            };
        }
        public override ElementTreeNode Clone()
        {
            return new RectNode(X, Y, Width, Height, Color);
        }
    }
}