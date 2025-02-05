

using System.Drawing;
using Nemo.Tools.ElementTreeObjects;

namespace Nemo.Tools.Drawing {
    public class Pencil : BaseTool
    {
        public bool startDraw { get; set; }
        public Point? previousPoint { get; set; }
        private ElementTreeDocument elementTreeDocument { get; set; }
        public Pencil(Canvas canvas, ElementTreeDocument etc): base(canvas)
        {
            elementTreeDocument = etc;
        }

        public override async Task End(Point point)
        {
            startDraw = false;
            previousPoint = null;
            
            elementTreeDocument
                .AddElementTreeObject(new DotElementObject(point.X, point.Y, "red"));
        }

        public override async Task Start(Point point)
        {
            previousPoint = point;
            startDraw = true;

            elementTreeDocument
                .AddElementTreeObject(new DotElementObject(point.X, point.Y, "red"));
        }

        public override async Task OnMove(Point point) {
            if (!startDraw) {
                return;
            }

            if(previousPoint != null) {
                elementTreeDocument
                .AddElementTreeObject(new LineElementObject(
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