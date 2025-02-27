

using System.Drawing;
using Nemo.Tools.ElementTreeNodes;

namespace Nemo.Tools.Drawing {
    public class Pencil : BaseTool
    {
        public bool startDraw { get; set; }
        public Point? previousPoint { get; set; }
        public Pencil(Canvas canvas): base(canvas)
        {
        }

        public override async Task End(Point point)
        {
            startDraw = false;
            previousPoint = null;
            
            _canvas
                .AddElementTreeObject(new DotNode(point.X, point.Y, "red"));
        }

        public override async Task Start(Point point)
        {
            previousPoint = point;
            startDraw = true;

            _canvas
                .AddElementTreeObject(new DotNode(point.X, point.Y, "red"));
        }

        public override async Task OnMove(Point point) {
            if (!startDraw) {
                return;
            }

            if(previousPoint != null && !(previousPoint.Value.X == point.X && previousPoint.Value.Y == point.Y)) {
                _canvas
                .AddElementTreeObject(new LineNode(
                    previousPoint.Value.X,
                    previousPoint.Value.Y,
                    point.X,
                    point.Y,
                    "red"
                ));
                previousPoint = point;
            }
        }

        public override Task Cancel()
        {
            startDraw = false;
            previousPoint = null;
            return Task.CompletedTask;
        }
    }
}