
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
                new AddElementCommand(
                    "rect", elementId,
                    new {
                        x = coords.X,
                        y = coords.Y,
                        width = coords.Width,
                        height = coords.Height,
                        style="fill:transparent;stroke:red;stroke-width:2"
                    }
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
                new AddElementCommand(
                    "rect", "shadowRect1",
                    new {
                        x = coords.X,
                        y = coords.Y,
                        width = coords.Width,
                        height = coords.Height,
                        style="fill:transparent;stroke:black;stroke-width:1"
                    }
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