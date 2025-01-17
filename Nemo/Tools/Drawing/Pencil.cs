

using System.Drawing;
using Nemo.Tools.CanvasCommands;

namespace Nemo.Tools.Drawing {
    public class Pencil : BaseTool
    {
        private readonly Canvas _canvas;
        public bool startDraw { get; set; }
        public Point? previousPoint { get; set; }
        public Pencil()
        {
        }

        public override IEnumerable<CanvasCommand> End(Point point)
        {
            startDraw = false;
            previousPoint = null;
            var commands = new List<CanvasCommand>();
            
            commands.Add(new DrawDotCommand( 
                    point.X,
                    point.Y,
                    "red"
                ));

            return commands;
        }

        public override IEnumerable<CanvasCommand> Start(Point point)
        {
            var commands = new List<CanvasCommand>();
            previousPoint = point;
            startDraw = true;

            commands.Add(new DrawDotCommand( 
                    point.X,
                    point.Y,
                    "red"
                ));

            return commands;
        }

        public override IEnumerable<CanvasCommand> OnMove(Point point) {
            if (!startDraw) {
                return new List<CanvasCommand>();
            }

            var commands = new List<CanvasCommand>();
            
            if(previousPoint != null) {
                commands.Add(new DrawLineCommand(
                        previousPoint.Value.X,
                        previousPoint.Value.Y,
                        point.X,
                        point.Y,
                        "red"
                    ));
                previousPoint = point;
            }

            return commands;
        }
    }
}