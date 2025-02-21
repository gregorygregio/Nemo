using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Nemo.Pages {
    public partial class Home {
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
        private Canvas? canvas { get; set;}
        public bool HasImageLoaded { get => canvas != null && canvas.Image != null && canvas.Image.IsLoaded; }
        private string _fileName = string.Empty;
        public string FileName { get => _fileName; set {
            _fileName = value;
            if(canvas != null && canvas.Image != null) {
                canvas.Image.FileName = value;
            }
        }}
        public string MouseCursor { get {
            return canvas?.CursorType != null ? canvas.CursorType : "default";
        } set {} }

        protected override async Task OnInitializedAsync()
        {
            canvas = new Canvas(_jsRuntime);
            var lDotNetReference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("GLOBAL.SetDotnetReference", lDotNetReference);
        }

        [JSInvokable]
        public async Task ElementClicked(string elementId) 
        {
            if(canvas == null) {
                return;
            }
            await canvas.ElementClicked(elementId);
        }

        [JSInvokable]
        public void OnImageRendered(ImageSize imageSize) {
            if(canvas == null) {
                return;
            }
            Task.Run(() => canvas.OnImageRendered(imageSize.width, imageSize.height));
        }

        public record ImageSize(int width, int height);

        public async void LoadImage(InputFileChangeEventArgs e) {
            if(canvas == null) {
                return;
            }
            await canvas.LoadImage(e.File);
            _fileName = canvas.Image != null ? canvas.Image.FileName : string.Empty;
            base.StateHasChanged();
        }

        public void SelectTool(string tool) {
            canvas.SelectTool(tool);
        }

        public bool startDraw { get; set; }
        public Point? previousPoint { get; set; }
        public async Task StartToolAction(MouseEventArgs e) {
            if(e.Button != 0 /* LEFT mouse button */) {
                return;
            }
            await canvas.StartToolAction(e);
        }
        public async Task CancelToolAction(MouseEventArgs e) {
            await canvas.CancelToolAction(e);
        }

        public async Task MoveTool(MouseEventArgs e) {
            await canvas.MoveTool(e);
        }

        public async Task EndToolAction(MouseEventArgs e) {
            await canvas.EndToolAction(e);
        }

        public async Task Export() {
            //await _jsRuntime.InvokeVoidAsync("downloadImage", canvas.FileName, canvas.ContentType);
            await Task.CompletedTask;
        }

        private void ClearCanvas()
        {
            Task.Run(() => _jsRuntime.InvokeVoidAsync("clearCanvas", canvas.Width, canvas.Height));
        }

        public void Undo() {
            canvas.Undo();
        }
    }
}
