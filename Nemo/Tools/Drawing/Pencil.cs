

using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools.Drawing {
    public class Pencil : BaseTool
    {
        private readonly Canvas _canvas;
        public bool startDraw { get; set; }
        public Point? previousPoint { get; set; }
        private string elementId { get; set; } = string.Empty;
        public Pencil(Canvas canvas)
        {
            _canvas = canvas;
        }

        public override IEnumerable<CanvasCommand> End(Point point)
        {
            startDraw = false;
            previousPoint = null;
            elementId = string.Empty;
            return new List<CanvasCommand>();
        }

        public override IEnumerable<CanvasCommand> Start(Point point)
        {
            var commands = new List<CanvasCommand>();
            previousPoint = point;
            startDraw = true;

            elementId = string.Concat("circle_", Guid.NewGuid().ToString());
            commands.Add(new AddElementCommand("circle", elementId, 
                new {
                    cx   = point.X,
                    cy   = point.Y,
                    fill = "red",
                    r    = 2,
                }));

            return commands;
        }

        public override IEnumerable<CanvasCommand> OnMove(Point point) {
            if (!startDraw) {
                return new List<CanvasCommand>();
            }

            var commands = new List<CanvasCommand>();
            
            if(string.IsNullOrEmpty(elementId)) {
                elementId = string.Concat("line_", Guid.NewGuid().ToString());
            }
            
            if(previousPoint != null) {
                commands.Add(new AddElementCommand("line", elementId,
                    new 
                    {
                        x1   = previousPoint.Value.X,
                        y1   = previousPoint.Value.Y,
                        x2   = point.X,
                        y2   = point.Y,
                        style = "stroke: red; stroke-width: 4px"
                    }));
                previousPoint = point;
            }

            return commands;
        }
    }
}