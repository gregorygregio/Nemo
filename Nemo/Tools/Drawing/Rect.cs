using System.Drawing;
using Nemo.Tools.ElementTreeNodes;

namespace Nemo.Tools.Drawing
{
    internal record RectShape(int X, int Y, int Width, int Height) {}
    public class Rect : BaseTool
    {
        public Rect(Canvas canvas): base(canvas)
        {
        }
        private Point? startingPoint { get; set; }
        private bool hasShadowRectDrawn { get; set; } = false;
        private RectShape GetRectShape(Point a, Point b) {
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
    
            return new RectShape(minX, minY, maxX - minX, maxY - minY);
        }
        public override async Task End(Point point)
        {
            if(!startingPoint.HasValue) {
                return;
            }

            if(hasShadowRectDrawn) {
                await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowRect1" });
            }
            
            var coords = GetRectShape(point, startingPoint.Value);

            _canvas
                .AddElementTreeObject(new RectNode(coords.X, coords.Y, coords.Width, coords.Height, "red"));

            startingPoint = null;
            hasShadowRectDrawn = false;
        }

        public override async Task OnMove(Point point)
        {
            if(!startingPoint.HasValue) {
                return;
            }

            if(hasShadowRectDrawn) {
                await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowRect1" });
            }

            var coords = GetRectShape(point, startingPoint.Value);
            
            await _canvas.ExecuteAction("addSvgElement", new object[] {
                "rect", "shadowRect1",
                new {
                    x=coords.X, y=coords.Y, width=coords.Width, height=coords.Height, 
                    style="stroke: red; stroke-width: 1; fill: none"
                }
            });

            hasShadowRectDrawn = true;
        }

        public override async Task Start(Point point)
        {
            startingPoint = point;
        }
        public override async Task Cancel()
        {
            await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowRect1" });
            startingPoint = null;
        }
    }
}