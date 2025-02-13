namespace Nemo.Tools.ElementTreeObjects
{
    public class DotElementObject: ElementTreeNode
    {
        public DotElementObject(int x, int y, string color)
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
        public override object[] GetElementParams() {
            return new object[3] {
                X, Y, Color
            };
        }
    }
}