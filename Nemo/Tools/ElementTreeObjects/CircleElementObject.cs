namespace Nemo.Tools.ElementTreeObjects
{
    public class CircleElementObject : ElementTreeObject
    {
        public CircleElementObject(int x, int y, int radius, string color)
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
        public override object[] GetElementParams()
        {
            return new object[4] {
                X, Y, Radius, Color
            };
        }
    }
}