using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Nemo.Tools;
using Nemo.Tools.Drawing;
using Nemo.Tools.ElementTreeObjects;

namespace Nemo
{
    public class Canvas {
        private IJSRuntime _jsRuntime { get; set; }
        private const long maxAllowedSize = 1024 * 1024 * 1024; //1GB
        public CanvasImage? Image { get; set; }
        public int Width { get => Image != null ? Image.Width : 0; }
        public int Height { get => Image != null ? Image.Height : 0; }
        public Canvas(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            rootElementTreeNode = new ElementTreeNode(true);
            CurrentElement = rootElementTreeNode;
        }
        private ITool? selectedTool { get; set; }
        public string CursorType { get; set; } = "default";
        private ElementTreeNode rootElementTreeNode { get; set; }
        public ElementTreeNode CurrentElement { get; set; }

        #region Tool Actions
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
        #endregion

        #region JsInterop Actions
        public async Task ExecuteAction(string action, object?[]? args) {
            await _jsRuntime.InvokeVoidAsync(action, args);
        }
        public record BatchCanvasAction(string action, object?[]? args);
        public async Task ExecuteActionBatch(List<BatchCanvasAction> actions) {
            await _jsRuntime.InvokeVoidAsync("executeBatchActions", actions);
        }
        #endregion

        public async Task OnImageRendered(int width, int height) {
            Image.Width = width;
            Image.Height = height;
            if(!rootElementTreeNode.Rendered) {
                await RenderElement(rootElementTreeNode);
            }
        }
        public async Task LoadImage(IBrowserFile file) {
            using var readStream = file.OpenReadStream(maxAllowedSize: maxAllowedSize);
            
            var _contentType = file.ContentType;
            if(_contentType != "image/png" && _contentType != "image/jpeg") {
                Console.WriteLine("Invalid file type");
                await _jsRuntime.InvokeVoidAsync("displayErrorMessage", "Invalid file type");    
                return;
            }

            Image = new CanvasImage(file.Name, _contentType);

            await Image.LoadImage(readStream);
            
            await RenderImage(Image);
        }
        public async Task RenderImage(CanvasImage img) {
            if(img.ImageData == null) {
                return;
            }
            var strRef = new DotNetStreamReference(new MemoryStream(img.ImageData));
            await _jsRuntime.InvokeVoidAsync("setSource", strRef, img.ContentType);
        }

        public async Task Redraw() {
            if(Image == null) {
                return;
            }
            SetNodesRendered(rootElementTreeNode, false);
            await ExecuteAction("clearCanvas", new object[] { Image.Width, Image.Height });
            await RenderImage(Image);
        }

        public void SetNodesRendered(ElementTreeNode elm, bool render) {
            elm.Rendered = render;
            if(elm.Next != null) {
                SetNodesRendered(elm.Next, render);
            }
        }

        public void AddElementTreeObject(ElementTreeNode element) {
            CurrentElement.Next = element;
            CurrentElement = CurrentElement.Next;
            Task.Run(() => RenderElement(element));
        }

        public async Task RenderElement(ElementTreeNode element) {
            if(element.Rendered) {
                return;
            }

            if(!string.IsNullOrEmpty(element.GetElementAction())) {
                await ExecuteAction(element.GetElementAction(), element.GetElementParams());
            }

            element.Rendered = true;
            if(element.Next != null) {
                await RenderElement(element.Next);
            }
        }
    }
}