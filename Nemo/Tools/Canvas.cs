
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Nemo.Tools.Drawing;

namespace Nemo.Tools
{
    public class Canvas {
        private IJSRuntime _jsRuntime { get; set; }
        private const long maxAllowedSize = 1024 * 1024 * 1024; //1GB
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 600;
        public bool HasImageLoaded { get; set; } = false;
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public Canvas(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        private ITool? selectedTool { get; set; }
        public string CursorType { get; set; } = "default";
        public void SelectTool(string tool) {
            switch (tool.ToLower())
            {
                case "pencil":
                    selectedTool = new Pencil(this);
                    CursorType = "cell";
                    break;
                case "rect":
                    selectedTool = new Rect(this);
                    CursorType = "cell";
                    break;
                case "circle":
                    selectedTool = new Circle(this);
                    CursorType = "cell";
                    break;
                case "eraser":
                    selectedTool = new Eraser(this);
                    CursorType = "grabbing";
                    break;
                case "crop":
                    selectedTool = new Crop(this);
                    CursorType = "crosshair";
                    break;
                default:
                    selectedTool = null;
                    CursorType = "default";
                    break;
            }
        }

        public async Task ElementClicked(string elementId) {
            if(selectedTool == null) {
                return;
            }

            await selectedTool.OnElementClicked(elementId);
        }

        public async Task StartToolAction(MouseEventArgs e)
        {
            if(selectedTool == null) {
                return;
            }
            
            await selectedTool.Start(new System.Drawing.Point((int)e.OffsetX, (int)e.OffsetY));    
        }

        public async Task EndToolAction(MouseEventArgs e) {
            if(selectedTool == null) {
                return;
            }
            
            await selectedTool.End(new System.Drawing.Point((int)e.OffsetX, (int)e.OffsetY));
        }

        public async Task MoveTool(MouseEventArgs e)
        {
            if(selectedTool == null) {
                return;
            }

            await selectedTool.OnMove(new System.Drawing.Point((int)e.OffsetX, (int)e.OffsetY));
        }

        public async Task CancelToolAction(MouseEventArgs e) {
            if(selectedTool == null) {
                return;
            }

            await selectedTool.Cancel();
        }

        public async Task ExecuteAction(string action, object?[]? args) {
            await _jsRuntime.InvokeVoidAsync(action, args);
        }

        public async Task LoadImage(IBrowserFile file) {
            using var readStream = file.OpenReadStream(maxAllowedSize: maxAllowedSize);

            var _contentType = file.ContentType;
            if(_contentType != "image/png" && _contentType != "image/jpeg") {
                Console.WriteLine("Invalid file type");
                await _jsRuntime.InvokeVoidAsync("displayErrorMessage", "Invalid file type");    
                return;
            }

            FileName = file.Name;
            ContentType = _contentType;

            HasImageLoaded = true;
            await SetImage(readStream);
        }

        public async Task<Stream> GetImage() {
            var imageStream = await _jsRuntime.InvokeAsync<IJSStreamReference>("getImageData");
            using var stream = await imageStream.OpenReadStreamAsync();
            return stream;
            //var imageBytes = System.Convert.FromBase64String();
            //Console.WriteLine(imageBytes);
        }

        public async Task SetImage(Stream imageStream) {
            Console.WriteLine("Setting image " + imageStream.Length);
            Console.WriteLine(imageStream.Length);
            HasImageLoaded = true;
            var strRef = new DotNetStreamReference(imageStream);
            await _jsRuntime.InvokeVoidAsync("setSource", "baseImage", strRef, ContentType);

        }

    }
}