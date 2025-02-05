namespace Nemo.Tools.ElementTreeObjects
{
    public class ImageElementObject : ElementTreeObject
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[]? ImageData { get; set; }
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public int ImageOffsetX { get; set; } = 0;
        public int ImageOffsetY { get; set; } = 0;
    }
}