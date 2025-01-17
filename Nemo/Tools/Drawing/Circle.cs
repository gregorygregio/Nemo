using Nemo.Tools.CanvasCommands;
using System.Drawing;

namespace Nemo.Tools.Drawing
{
    public record CircleCoords(int X, int Y, double Radius);
    public class Circle : BaseTool
    {
        private Point? startingPoint { get; set; }

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
        public override IEnumerable<CanvasCommand> End(Point point)
        {
            var commands = new List<CanvasCommand>();
            if(!startingPoint.HasValue) {
                return commands;
            }

            var coords = GetCircleCoords(point, startingPoint.Value);

            commands.Add(new DrawCircleCommand
            (
                    coords.X,
                    coords.Y,
                    (int)coords.Radius,
                    "red"
            ));

            startingPoint = null;
            
            return commands;
        }

        public override IEnumerable<CanvasCommand> OnMove(Point point)
        {
            var commands = new List<CanvasCommand>();
            if(!startingPoint.HasValue) {
                return commands;
            }

            var coords = GetCircleCoords(point, startingPoint.Value);

            commands.Add(new RemoveElementCommand("circleShadow"));

            commands.Add(new DrawCircleCommand
            (
                    coords.X,
                    coords.Y,
                    (int)coords.Radius,
                    "red"
            ));
                
            return commands;
        }

        public override IEnumerable<CanvasCommand> Start(Point point)
        {
            var commands = new List<CanvasCommand>();
            startingPoint = point;
            return commands;
        }
    }
}