namespace Nemo.Tools.ElementTreeNodes
{
    public class DotNode: ElementTreeNode
    {
        public DotNode(int x, int y, string color)
        {
            X = x;
            Y = y;
            Color = color;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }

        public override string GetElementAction() {
            return "drawDot";
        }
        public override object[] GetElementParams(int offsetX, int offsetY) {
            return new object[3] {
                X + offsetX, Y + offsetY, Color
            };
        }
    }
}