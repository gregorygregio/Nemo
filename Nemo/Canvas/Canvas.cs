using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Nemo.Tools;
using Nemo.Tools.Drawing;
using Nemo.Tools.ElementTreeNodes;

namespace Nemo
{
    public class Canvas {
        private IJSRuntime _jsRuntime { get; set; }
        private const long maxAllowedSize = 1024 * 1024 * 1024; //1GB
        public CanvasImage? Image { get; set; }
        public int Width { get => rootFrameNode.Width; }
        public int Height { get => rootFrameNode.Height; }
        public Canvas(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            rootFrameNode = new FrameNode();
            CurrentElement = rootFrameNode;
        }
        private ITool? selectedTool { get; set; }
        public string CursorType { get; set; } = "default";
        private FrameNode rootFrameNode { get; set; }
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

        // public async Task ResizeCanvas(int width, int height) {
        //     Width = width;
        //     Height = height;
        //     await ExecuteAction("resizeCanvas", new object[] { width, height });
        // }
        public async Task OnImageRendered(int width, int height) {
            Image.Width = width;
            Image.Height = height;
            if(rootFrameNode.Width == 0) {
                rootFrameNode.Width = width;
            }
            if(rootFrameNode.Height == 0) {
                rootFrameNode.Height = height;
            }
            
            if(!rootFrameNode.Rendered) {
                await RenderBatchOfElements(rootFrameNode, rootFrameNode.OffsetX, rootFrameNode.OffsetY);
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

            rootFrameNode.Width = 0;
            rootFrameNode.Height = 0;
            Image = new CanvasImage(file.Name, _contentType);

            await Image.LoadImage(readStream);
            
            await RenderImage(Image);
        }
        public async Task RenderImage(CanvasImage img) {
            if(img.ImageData == null) {
                return;
            }
            var strRef = new DotNetStreamReference(new MemoryStream(img.ImageData));
            await _jsRuntime.InvokeVoidAsync("setSource", strRef, img.ContentType, img.OffsetX, img.OffsetY, rootFrameNode.Width, rootFrameNode.Height);
        }

        public async Task Redraw(int offsetX, int offsetY, int width, int height) {
            if(Image == null) {
                return;
            }

            var currentWidth = Width;
            var currentHeight = Height;

            Image.OffsetX -= offsetX;
            Image.OffsetY -= offsetY;
            rootFrameNode.OffsetX = Image.OffsetX;
            rootFrameNode.OffsetY = Image.OffsetY;
            rootFrameNode.Width = width;
            rootFrameNode.Height = height;

            SetNodesRendered(rootFrameNode, false);
            await ExecuteAction("clearCanvas", new object[] { currentWidth, currentHeight });
            await RenderImage(Image);
        }

        public void SetNodesRendered(ElementTreeNode elm, bool render) {
            foreach(var node in elm.GetNodes()) {
                node.Rendered = render;
            }
        }

        public void AddElementTreeObject(ElementTreeNode element) {
            CurrentElement.Next = element;
            while(CurrentElement.Next != null) {
                CurrentElement = CurrentElement.Next;
            }
            Task.Run(() => RenderBatchOfElements(element));
        }

        public async Task RenderBatchOfElements(ElementTreeNode? elm, int offsetX=0, int offsetY=0) {
            
            foreach(var actions in GetBatchesOfCanvasActions(elm, offsetX, offsetY)) {
                Console.WriteLine("Sending batch of actions");
                await ExecuteActionBatch(actions);
            }
        }
        private IEnumerable<List<BatchCanvasAction>> GetBatchesOfCanvasActions(ElementTreeNode? elm, int offsetX=0, int offsetY=0)
        {
            var actions = new List<BatchCanvasAction>();
            int actionsCount = 0;
            foreach(var node in elm?.GetNodes()) {
                actions.Add(new BatchCanvasAction(node.GetElementAction(), node.GetElementParams(offsetX, offsetY)));
                actionsCount++;
                if(actionsCount >= 1000) {
                    yield return actions;
                    actions = new List<BatchCanvasAction>();
                    actionsCount = 0;
                }
            }
            yield return actions;
        }
    }
}