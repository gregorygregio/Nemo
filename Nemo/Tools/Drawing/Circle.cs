using Nemo.Tools.ElementTreeNodes;
using System.Drawing;

namespace Nemo.Tools.Drawing
{
    public record CircleCoords(int X, int Y, double Radius);
    public class Circle : BaseTool
    {
        private Point? startingPoint { get; set; }
        public Circle(Canvas canvas) : base(canvas)
        {
        }
        private CircleCoords GetCircleCoords(Point a, Point b) 
        {
            int minX, maxX, minY, maxY;
            if(a.X > b.X) {
                maxX = a.X;
                minX = b.X;
            }
            else 
            {
                minX = a.X;
                maxX = b.X;
            }

            if(a.Y > b.Y) {
                maxY = a.Y;
                minY = b.Y;
            }
            else
            {
                minY = a.Y;
                maxY = b.Y;
            }


            var radius = Math.Sqrt(Math.Pow(maxX - minX, 2) + Math.Pow(maxY - minY, 2)) / 2;
            var xPosition = (maxX + minX) / 2;
            var yPosition = (maxY + minY) / 2;

            return new CircleCoords(xPosition, yPosition, radius);

        }
        public override async Task End(Point point)
        {
            if(!startingPoint.HasValue) {
                return;
            }

            await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowCircle" });

            var coords = GetCircleCoords(point, startingPoint.Value);

            _canvas
                .AddElementTreeObject(new CircleNode(coords.X, coords.Y, (int)coords.Radius, "red"));

            startingPoint = null;
        }

        public override async Task OnMove(Point point)
        {
            if(!startingPoint.HasValue) {
                return;
            }

            await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowCircle" });

            var coords = GetCircleCoords(point, startingPoint.Value);

            await _canvas.ExecuteAction("addSvgElement", new object[] {
                "circle", "shadowCircle",
                new {
                    cx=coords.X, cy=coords.Y, r=coords.Radius, 
                    style="stroke: red; stroke-width: 1; fill: none"
                }
            });

        }

        public override async Task Start(Point point)
        {
            startingPoint = point;
        }

        public override async Task Cancel()
        {
            await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowCircle" });
            startingPoint = null;
        }
    }
}