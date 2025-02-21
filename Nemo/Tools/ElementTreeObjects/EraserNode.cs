namespace Nemo.Tools.ElementTreeNodes
{
    public class EraserNode : ElementTreeNode
    {
        public EraserNode(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public override string GetElementAction()
        {
            return "clearRect";
        }
        public override object[] GetElementParams(int offsetX, int offsetY)
        {
            return new object[4] {
                X + offsetX, Y + offsetY, Width, Height
            };
        }
        public override ElementTreeNode Clone()
        {
            return new EraserNode(X, Y, Width, Height);
        }
    }
}