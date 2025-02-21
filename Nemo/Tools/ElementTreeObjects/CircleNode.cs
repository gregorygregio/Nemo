namespace Nemo.Tools.ElementTreeNodes
{
    public class CircleNode : ElementTreeNode
    {
        public CircleNode(int x, int y, int radius, string color)
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }
        public string Color { get; set; }

        public override string GetElementAction()
        {
            return "drawCircle";
        }
        public override object[] GetElementParams(int offsetX, int offsetY)
        {
            return new object[4] {
                X + offsetX, Y + offsetY, Radius, Color
            };
        }
        public override ElementTreeNode Clone()
        {
            return new CircleNode(X, Y, Radius, Color);
        }
    }
}