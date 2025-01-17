
using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools.Drawing
{
    internal record RectCoords(int X, int Y, int Width, int Height) {}
    public class Rect : BaseTool
    {
        private Point? startingPoint { get; set; }
        private bool hasShadowRectDrawn { get; set; } = false;
        private RectCoords GetRectCoords(Point a, Point b) {
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

            if(a.Y > startingPoint.Value.Y) {
                maxY = a.Y;
                minY = b.Y;
            }
            else 
            {
                minY = a.Y;
                maxY = b.Y;
            }
    
            return new RectCoords(minX, minY, maxX - minX, maxY - minY);
        }
        public override IEnumerable<CanvasCommand> End(Point point)
        {
            var commands = new List<CanvasCommand>();

            if(!startingPoint.HasValue) {
                return commands;
            }

            if(hasShadowRectDrawn) {
                commands.Add(new RemoveElementCommand("shadowRect1"));
            }
            
            var coords = GetRectCoords(point, startingPoint.Value);
            string elementId = string.Concat("rect_", Guid.NewGuid().ToString());

            commands.Add(
                new DrawRectCommand(
                    coords.X,
                    coords.Y,
                    coords.Width,
                    coords.Height,
                    "red"
                )
            );
            startingPoint = null;
            hasShadowRectDrawn = false;
            return commands;
        }

        public override IEnumerable<CanvasCommand> OnMove(Point point)
        {
            var commands = new List<CanvasCommand>();
            if(!startingPoint.HasValue) {
                return commands;
            }

            if(hasShadowRectDrawn) {
                commands.Add(new RemoveElementCommand("shadowRect1"));
            }

            var coords = GetRectCoords(point, startingPoint.Value);

            commands.Add(
                new DrawRectCommand(
                    coords.X,
                    coords.Y,
                    coords.Width,
                    coords.Height,
                    "gray"
                )
            );

            hasShadowRectDrawn = true;

            return commands;
        }

        public override IEnumerable<CanvasCommand> Start(Point point)
        {
            startingPoint = point;
            return new List<CanvasCommand>();
        }
    }
}