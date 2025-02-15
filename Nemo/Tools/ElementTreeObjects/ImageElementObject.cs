using Microsoft.JSInterop;

namespace Nemo.Tools.ElementTreeNodes
{
    public class ImageNode : ElementTreeNode
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[]? ImageData { get; set; }
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