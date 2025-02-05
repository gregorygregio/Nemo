using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Nemo.Tools;

namespace Nemo.Pages {
    public partial class Home {
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
        private Canvas canvas { get; set;}
        public bool HasImageLoaded { get => canvas.HasImageLoaded; }
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
            await canvas.ElementClicked(elementId);
        }

        [JSInvokable]
        public void SetImageSize(ImageSize imageSize) {
            Console.WriteLine("SetImageSize");
            Console.WriteLine(imageSize.width);
            Console.WriteLine(imageSize.height);
            Console.WriteLine(imageSize);

            canvas.Width = imageSize.width;
            canvas.Height = imageSize.height;
        }

        public record ImageSize(int width, int height);

        public async void LoadImage(InputFileChangeEventArgs e) {
            await canvas.LoadImage(e.File);
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
            _jsRuntime.InvokeVoidAsync("clearCanvas", canvas.Width, canvas.Height);
        }
    }
}
