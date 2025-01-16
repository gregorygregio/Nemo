using System.Drawing;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Nemo.Tools;

namespace Nemo.Pages {
    public partial class Home {
        private const long maxAllowedSize = 1024 * 1024 * 1024; //1GB
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
        private Canvas canvas { get; set;}
        private string fileName { get; set; }
        private string fileContentType { get; set; }

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

        public async void LoadImage(InputFileChangeEventArgs e) {
            using var readStream = e.File.OpenReadStream(maxAllowedSize: maxAllowedSize);

            var strRef = new DotNetStreamReference(readStream);
            
            fileName = e.File.Name;
            fileContentType = e.File.ContentType;

            await _jsRuntime.InvokeVoidAsync("setSource", "baseImage", strRef, e.File.ContentType, 
            e.File.Name);
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

        public async Task MoveTool(MouseEventArgs e) {
            await canvas.MoveTool(e);
        }

        public async Task EndToolAction(MouseEventArgs e) {
            await canvas.EndToolAction(e);
        }

        public async Task Export() {
            var svg = await _jsRuntime.InvokeAsync<string>("getSvg");
            var bytes = Encoding.UTF8.GetBytes(svg);
            await _jsRuntime.InvokeVoidAsync("downloadSvg", fileName, bytes);
            //var img = new Image() { Src = $"data:image/svg+xml;base64,{base64}" };
            //await _jsRuntime.InvokeVoidAsync("exportSvg");
        }

        private void ClearSvg()
        {
            _jsRuntime.InvokeVoidAsync("clearSvgElements");
        }
    }
}
