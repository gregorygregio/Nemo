using Microsoft.JSInterop;

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
        public override string GetElementAction() {
            return "setSource";
        }
        public override object[] GetElementParams() {
            if(ImageData == null) {
                return new object[2] {
                    new DotNetStreamReference(new MemoryStream(new byte[0])), ContentType
                };
            }
            var strRef = new DotNetStreamReference(new MemoryStream(ImageData));
            return new object[2] {
                strRef, ContentType
            };
        }
    }
}