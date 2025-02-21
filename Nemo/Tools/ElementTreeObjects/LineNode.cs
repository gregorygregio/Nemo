namespace Nemo.Tools.ElementTreeNodes
{
    public class LineNode : ElementTreeNode
    {
        public LineNode(int x1, int y1, int x2, int y2, string color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Color = color;
        }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public string Color { get; set; }

        public override string GetElementAction()
        {
            return "drawLine";
        }
        public override object[] GetElementParams(int offsetX, int offsetY)
        {
            return new object[5] {
                X1 + offsetX, Y1 + offsetY, X2 + offsetX, Y2 + offsetY, Color
            };
        }
        public override ElementTreeNode Clone()
        {
            return new LineNode(X1, Y1, X2, Y2, Color);
        }
    }
}