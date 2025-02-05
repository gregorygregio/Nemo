using System.Drawing;

namespace Nemo.Tools.Drawing
{
    public class Crop : BaseTool
    {
        public Rectangle CropArea { get; private set; }
        private Point? startingPoint { get; set; }
        private bool hasShadowRectDrawn { get; set; } = false;
        public Crop(Canvas canvas) : base(canvas)
        {
        }


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

        private void CreateRectangle(int x, int y, int width, int height)
        {
            CropArea = new Rectangle(x, y, width, height);
        }

        public override async Task End(Point point)
        {
            Console.WriteLine("On crop end");
            if(!startingPoint.HasValue) {
                return;
            }

            if(hasShadowRectDrawn) {
                await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowCrop" });
            }
            
            var coords = GetRectCoords(point, startingPoint.Value);
            
        }

        public override async Task OnMove(Point point)
        {
            if(!startingPoint.HasValue) {
                return;
            }

            if(hasShadowRectDrawn) {
                await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowCrop" });
            }

            var coords = GetRectCoords(point, startingPoint.Value);
            
            await _canvas.ExecuteAction("addSvgElement", new object[] {
                "rect", "shadowCrop",
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
            await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowCrop" });
            startingPoint = null;
        }
    }
}