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
                Console.WriteLine("!startingPoint.HasValue");
                return;
            }

            if(hasShadowRectDrawn) {
                await _canvas.ExecuteAction("removeSvgElement", new object[] { "shadowCrop" });
            }
            
            var coords = GetRectCoords(point, startingPoint.Value);

            //CreateRectangle(coords.X, coords.Y, coords.Width, coords.Height);
            Console.WriteLine("geting image");
            using var jsStream = await _canvas.GetImage();
            using var imageStream = new MemoryStream();
            await jsStream.CopyToAsync(imageStream);

            _canvas.HasImageLoaded = false;
            var outputStream = new MemoryStream();

            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            long readPosition = imageStream.Length;
            Console.WriteLine("readPosition: " + readPosition);

            do{
                long newPosition = Math.Max(0, readPosition - bufferSize);
                int readSize = (int)(readPosition - newPosition);
                imageStream.Position = newPosition;
                int bytesRead = await imageStream.ReadAsync(buffer, 0, readSize);
                Console.WriteLine("bytesRead: " + bytesRead);
                for (int i = bytesRead-1; i >= 0; --i)
                {
                    outputStream.WriteByte(buffer[i]);
                }
                readPosition = newPosition;
            } while(readPosition > 0);

            await outputStream.FlushAsync();
            outputStream.Seek(0, SeekOrigin.Begin);

            await _canvas.SetImage(outputStream);
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